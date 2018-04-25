// ReSharper disable once CheckNamespace

using System;
using System.Linq;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace Mono.Cecil.Fluent
{
    public static partial class CecilExtensions
    {
        public static MethodReference MakeGeneric(this MethodReference self, params TypeReference[] arguments)
        {
            if (self is GenericInstanceMethod closedInstance)
            {
                self = closedInstance.ElementMethod;
            }
            
            if (self.HasGenericParameters)
            {
                var genericInstance = new GenericInstanceMethod(self);

                foreach (var argument in arguments)
                    genericInstance.GenericArguments.Add(argument);

                return genericInstance;
            }

            var reference = new MethodReference(self.Name, self.ReturnType)
            {
                DeclaringType = self.DeclaringType.MakeGeneric(arguments),
                HasThis = self.HasThis,
                ExplicitThis = self.ExplicitThis,
                CallingConvention = self.CallingConvention,
            };

            foreach (var parameter in self.Parameters)
                reference.Parameters.Add(new ParameterDefinition(parameter.ParameterType));

            foreach (var generic_parameter in self.GenericParameters)
                reference.GenericParameters.Add(new GenericParameter(generic_parameter.Name, reference));

            return reference;
        }

        public static TypeReference GetGenericParameterType(this MethodReference method, int index)
        {
            var param = method.Parameters[index];
            var paramType = param.ParameterType;

            if (method is GenericInstanceMethod gMethod && paramType is GenericInstanceType gType)
            {
                var newGType = new GenericInstanceType(gType.ElementType);
                foreach (var genericArgument in gType.GenericArguments)
                {
                    var name = genericArgument.FullName;
                    if (name.StartsWith("!!"))
                    {
                        var mIndex = int.Parse(name.Substring(2));
                        newGType.GenericArguments.Add(gMethod.GenericArguments[mIndex]);
                    }
                    else if (name.StartsWith("!"))
                    {
                        var tIndex = int.Parse(name.Substring(1));
                        var decType = (GenericInstanceType)gMethod.DeclaringType;

                        newGType.GenericArguments.Add(decType.GenericArguments[tIndex]);
                    }
                }

                return newGType;
            }

            return paramType;
        }

        public static TypeReference GetDelegateType(this MethodReference method)
        {
            var module = method.Module;

            if (method.ReturnType == module.TypeSystem.Void)
            {
                if (method.Parameters.Count == 0)
                {
                    return module.SafeImport<Action>();
                }

                TypeReference openType = null;

                if (method.Parameters.Count == 1) openType = module.SafeImportOpen<Action<int>>();
                else if (method.Parameters.Count == 2) openType = module.SafeImportOpen<Action<int, int>>();
                else if (method.Parameters.Count == 3) openType = module.SafeImportOpen<Action<int, int, int>>();
                else if (method.Parameters.Count == 4) openType = module.SafeImportOpen<Action<int, int, int, int>>();

                var instance = new GenericInstanceType(openType);

                foreach (var argument in method.Parameters.Select(p => p.ParameterType))
                    instance.GenericArguments.Add(argument);
                return instance;
            }
            else
            {
                TypeReference openType = null;
                if (method.Parameters.Count == 0) openType = module.SafeImportOpen<Func<int>>();

                if (method.Parameters.Count == 1) openType = module.SafeImportOpen<Func<int, int>>();
                if (method.Parameters.Count == 2) openType = module.SafeImportOpen<Func<int, int, int>>();
                if (method.Parameters.Count == 3) openType = module.SafeImportOpen<Func<int, int, int, int>>();
                if (method.Parameters.Count == 4) openType = module.SafeImportOpen<Func<int, int, int, int, int>>();

                var instance = new GenericInstanceType(openType);
                foreach (var argument in method.Parameters.Select(p => p.ParameterType))
                    instance.GenericArguments.Add(argument);

                instance.GenericArguments.Add(method.ReturnType);
                return instance;
            }

            throw new ArgumentException();
        }

        public static Instruction LastRet(this MethodDefinition self)
        {
            if (self.Body.Instructions.Count == 0) return null;
            return self.Body.Instructions.LastOrDefault(p => p.OpCode == OpCodes.Ret);
        }
    }
}