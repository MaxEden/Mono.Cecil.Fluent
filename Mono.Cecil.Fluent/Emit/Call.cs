using System;
using System.Linq;
using Mono.Cecil.Cil;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
    partial class FluentEmitter
    {
        public FluentEmitter Call(MethodReference m)
        {
            if (m.Resolve().IsVirtual)
            {
                return Emit(OpCodes.Callvirt, m);
            }
            else
            {
                return Emit(OpCodes.Call, m);
            }
        }

        public FluentEmitter EqualsCall(TypeDefinition type)
        {
            var opEquality = type.Methods.FirstOrDefault(p => p.Name == "op_Equality");
            var objEquals = Module.SafeImport<object>(p => Object.Equals(null, null)).Resolve();

            if (opEquality != null)
            {
                Call(opEquality);
            }
            else if (type.IsStruct())
            {
                Call(objEquals);
            }
            else
            {
                Emit(OpCodes.Ceq);
            }

            return this;
        }

        public FluentEmitter EqualsStr()
        {
            return EqualsCall(Module.TypeSystem.String.Resolve());
        }
    }
}