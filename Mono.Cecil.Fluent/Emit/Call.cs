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

            var objEquals = Module.SafeImport<object>(p => Equals(null, null)).Resolve();
            if (type == null)
            {
                Call(objEquals);
                return this;
            }

            var opEquality = type.Methods.FirstOrDefault(p => p.Name == "op_Equality");
            if (type.IsPrimitive || type.IsEnum)
            {
                Emit(OpCodes.Ceq);
            }
            else
            {
                if (opEquality != null)
                {
                    Call(opEquality);
                }
                else if(!type.IsStruct())
                {
                    Call(objEquals);
                }
                else
                {
                    throw new ArgumentException();
                }
            }

            return this;
        }

        public FluentEmitter EqualsStr()
        {
            return EqualsCall(Module.TypeSystem.String.Resolve());
        }
    }
}