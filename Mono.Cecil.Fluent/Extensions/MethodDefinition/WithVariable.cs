using Mono.Cecil.Cil;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
    public static partial class MethodDefinitionExtensions
    {
        public static MethodDefinition WithVariable(this MethodDefinition     method,
                                                    SystemTypeOrTypeReference varType, string name,
                                                    out VariableDefinition    variableDefinition)
        {
            var type = varType.GetTypeReference(method.GetModule());
            var var = new VariableDefinition(type);

            WithVariable(method, name, var);

            variableDefinition = var;
            return method;
        }

        public static MethodDefinition WithVariable<T>(this MethodDefinition   method, string name,
                                                       out  VariableDefinition variableDefinition)
        {
            return WithVariable(method, typeof(T), name, out variableDefinition);
        }

        public static MethodDefinition WithVariable(this MethodDefinition method, string name, VariableDefinition var)
        {
            method.Body.Variables.Add(var);
            if (!string.IsNullOrEmpty(name))
            {
                method.DebugInformation.Scope?.Variables.Add(new VariableDebugInformation(var, name));
            }

            return method;
        }
    }
}