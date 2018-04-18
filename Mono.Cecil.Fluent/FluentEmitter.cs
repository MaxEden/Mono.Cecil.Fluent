using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using Mono.Collections.Generic;

namespace Mono.Cecil.Fluent
{
	public partial class FluentEmitter
	{
		public readonly ModuleDefinition Module;
	    public readonly AppendMode AppendMode;
        public readonly ILProcessor ILProcessor;

		/// <summary>
		/// Useful for Debugging.
		/// </summary>
		public string DisassembledBody => MethodDefinition.DisassembleBody();

		/// <summary>
		/// Useful for Debugging.
		/// </summary>
		public string DisassembledMethod => MethodDefinition.Disassemble();

		public readonly MethodDefinition MethodDefinition;

		public MethodBody Body => MethodDefinition.Body;

		public TypeDefinition DeclaringType
		{
			get { return MethodDefinition.DeclaringType; }
		}

		public TypeReference ReturnType
		{
			get { return MethodDefinition.ReturnType; }
		}

		public Collection<ParameterDefinition> Parameters => MethodDefinition.Parameters;

		public Collection<VariableDefinition> Variables => MethodDefinition.Body.Variables;

        public StackValidationMode StackValidationMode = Config.DefaultStackValidationMode;


	    internal FluentEmitter(MethodDefinition methodDefinition, AppendMode appendMode = AppendMode.Append, Instruction lastInstruction = null)
	    {
	        MethodDefinition = methodDefinition;
	        methodDefinition.Body.SimplifyMacros();
	        Module = methodDefinition.Module;

	        AppendMode = appendMode;
	        LastEmittedInstruction = lastInstruction;
	        ILProcessor = methodDefinition.Body.GetILProcessor();
	    }

	    public FluentEmitter SetStackValidationMode(StackValidationMode mode)
	    {
	        StackValidationMode = mode;
	        return this;
	    }

        public FluentEmitter WithVariable(SystemTypeOrTypeReference varType, string name = null)
        {
            MethodDefinition.WithVariable(varType, name, out var _);
            return this;
        }

        public FluentEmitter WithVariable<T>(string name = null)
        {
            MethodDefinition.WithVariable(typeof(T), name, out var _);
            return this;
        }

        public FluentEmitter WithVariable(VariableDefinition var)
        {
            Variables.Add(var);
            return this;
        }
        
        /// <summary>
        /// Compilation of MethodDefinition.Body and bind to given Func or Action signature.
        /// </summary>
        public T Compile<T>()
            where T : class
        {
            return MethodDefinition.Compile<T>();
        }

	    public string DisassembleBody()
	    {
	        return MethodDefinition.DisassembleBody();
	    }

        public System.Reflection.Emit.DynamicMethod ToDynamicMethod()
	    {
	        return MethodDefinition.ToDynamicMethod();
	    }

	    public MethodDefinition EndEmitting()
	    {
            MethodDefinition.Body.OptimizeMacros();
            // todo: something to do here? e.g. stack validation ..
	        return MethodDefinition;
	    }
    }
}
