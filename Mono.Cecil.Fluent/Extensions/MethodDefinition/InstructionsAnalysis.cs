// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace

using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Fluent
{
    partial class MethodDefinitionExtensions
    {
        public static List<MethodReference> GetCalledMethods(this MethodDefinition method)
        {
            if (method?.Body?.Instructions == null || method.Body.Instructions.Count == 0)
            {
                return new List<MethodReference>();
            }

            var result = method.Body.Instructions
                .Where(i => i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt)
                .Select(i => (MethodReference)i.Operand)
                .Distinct()
                .ToList();

            return result;
        }
    }
}