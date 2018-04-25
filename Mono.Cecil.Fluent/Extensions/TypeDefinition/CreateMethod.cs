using System;
using System.Linq;
using System.Linq.Expressions;
using Mono.Cecil.Fluent.Utils;
using Mono.Cecil.Rocks;

// ReSharper disable InconsistentNaming

namespace Mono.Cecil.Fluent
{
	public static partial class IMemberDefinitionExtensions
	{
		public static MethodDefinition CreateMethod(this TypeDefinition type, string name = null, SystemTypeOrTypeReference returnType = null, MethodAttributes? attributes = null)
		{
			var module = type.GetModule();
			var t = returnType != null ? returnType.GetTypeReference(type.GetModule()) : module.TypeSystem.Void;
			var method = new MethodDefinition(name ?? Generate.Name.ForMethod(), attributes ?? 0, t);

		    if(type is TypeDefinition definition) 
				definition.Resolve().Methods.Add(method);
			else
				type.DeclaringType.Methods.Add(method);

		    return method;
		}

		public static MethodDefinition CreateMethod<T>(this TypeDefinition type, string name = null, MethodAttributes? attributes = null)
		{
			return CreateMethod(type, name, typeof(T), attributes);
		}

		public static MethodDefinition CreateMethod(this TypeDefinition type, SystemTypeOrTypeReference returnType, MethodAttributes? attributes = null)
		{
			return CreateMethod(type, null, returnType.GetTypeReference(type.GetModule()), attributes);
		}

        public static MethodDefinition GetOrCreateStaticConstructor(this TypeDefinition type)
        {
            var staticCtor = type.GetStaticConstructor();
            if (staticCtor == null)
            {
                staticCtor = new MethodDefinition(
                    ".cctor",
                    MethodAttributes.Private
                    | MethodAttributes.HideBySig
                    | MethodAttributes.SpecialName
                    | MethodAttributes.RTSpecialName
                    | MethodAttributes.Static,
                    type.Module.TypeSystem.Void);
                type.Methods.Add(staticCtor);
            }

            return staticCtor;
        }

	    public static void ImplementInterface<T>(this TypeDefinition type, bool explicitly)
	    {
	        var interfaceRef = type.Module.SafeImport<T>();
	        if (type.Interfaces.Any(p => p.InterfaceType.FullName == interfaceRef.FullName)) return;

	        type.Interfaces.Add(new InterfaceImplementation(interfaceRef));

	        var interfaceDef = interfaceRef.Resolve();
	        foreach (var method in interfaceDef.Methods.Select(p=>type.Module.ImportReference(p)))
	        {
	            var methodRef = type.Module.ImportReference(method);
	            var name = method.Name;
	            if (explicitly) name = interfaceRef.FullName + "." + name;

	            if (type.Methods.Any(p => p.Name == name)) continue; //TODO compare by signature

	            var typeMethod = type.CreateMethod(name, method.ReturnType, method.Resolve().Attributes);
	            typeMethod.IsAbstract = false;
	            typeMethod.Parameters.AddRange(method.Parameters);

	            if (explicitly) typeMethod.Overrides.Add(method);
	        }
	    }

	    public static MethodDefinition GetMethod<T>(this TypeDefinition type, Expression<Action<T>> expression)
	    {
	        var refMethod = type.Module.SafeImport(expression);
	        if (refMethod.DeclaringType.FullName != type.FullName)
	        {
	            return type.Methods.FirstOrDefault(p => p.Overrides.Any(x=>x.FullName == refMethod.FullName));
	        }

	        return refMethod.Resolve();
	    }
    }


}
