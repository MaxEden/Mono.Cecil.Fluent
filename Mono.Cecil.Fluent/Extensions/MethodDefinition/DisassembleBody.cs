

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	public static partial class MethodDefinitionExtensions
	{
        public static string DisassembleBody(this MethodDefinition method)
        {
            return method.Body.Disassemble();
        }
	}
}
