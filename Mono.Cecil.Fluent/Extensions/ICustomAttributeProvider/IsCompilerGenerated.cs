using System;
using System.Linq;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
    public static partial class CecilExtensions
    {
        public static bool IsCompilerGenerated(this ICustomAttributeProvider provider)
        {
            if (provider != null && provider.HasCustomAttributes)
            {
                return provider.CustomAttributes.Any(a =>
                    a.AttributeType.FullName == "System.Runtime.CompilerServices.CompilerGeneratedAttribute");
            }

            return false;
        }

        public static bool HasCompilerGeneratedAttribute(this ICustomAttributeProvider provider)
        {
            return provider.IsCompilerGenerated();
        }

        public static bool HasAttribute(this ICustomAttributeProvider provider, string name)
        {
            return provider.HasCustomAttributes &&
                   provider.CustomAttributes.Any(p => p.AttributeType.Name == name);
        }

        public static bool HasAttribute<T>(this ICustomAttributeProvider provider) where T : Attribute
        {
            return provider.HasCustomAttributes &&
                   provider.CustomAttributes.Any(p => p.AttributeType.Name == typeof(T).Name);
        }

        public static void AddAttribute<T>(this ICustomAttributeProvider provider,
                                           ModuleDefinition              module,
                                           params object[]               args)
            where T : Attribute
        {
            var attrType = typeof(T);

            var constructors = attrType.GetConstructors();
            var constructor = constructors.FirstOrDefault(p =>
                p.GetParameters().Length == args.Length
                && (
                    p.GetParameters().Length == 0
                    ||
                    p.GetParameters()
                        .Select((info, i) => info.ParameterType.IsInstanceOfType(args[i])
                        || (info.ParameterType == typeof(Type) && args[i] is TypeReference))
                        .All(x => x)));

            if (constructor == null) throw new ArgumentException();

            var constructorRef = module.ImportReference(constructor);

            CustomAttribute attribute = new CustomAttribute(constructorRef);

            var argTypes = args.Select(p => p.GetType()).ToList();
            var argTypeRefs = new TypeReference[argTypes.Count];

            for (int i = 0; i < argTypes.Count; i++)
            {
                if (argTypes[i] == typeof(string))
                {
                    argTypeRefs[i] = module.TypeSystem.String;
                }
                else if (typeof(TypeReference).IsAssignableFrom(argTypes[i]))
                {
                    argTypeRefs[i] = module.SafeImport<Type>();
                }
                else if (argTypes[i] == typeof(Type))
                {
                    argTypeRefs[i] = module.SafeImport<Type>();
                    args[i] = module.SafeImport((Type)args[i]);
                }
                else
                {
                    argTypeRefs[i] = module.SafeImport(argTypes[i]);
                }


                attribute.ConstructorArguments.Add(new CustomAttributeArgument(argTypeRefs[i], args[i]));
            }

            provider.CustomAttributes.Add(attribute);
        }
    }
}