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
    }
}
