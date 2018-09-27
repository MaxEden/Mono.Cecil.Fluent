﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

// ReSharper disable MemberCanBePrivate.Global
namespace Mono.Cecil.Fluent
{
	partial class FluentEmitter
	{
		internal Instruction LastEmittedInstruction = null;
		internal readonly Queue<Func<FluentEmitter, bool>> PostEmitActions = new Queue<Func<FluentEmitter, bool>>(); 
	    private void EmitAction(Instruction instruction)
	    {
	        if (AppendMode == AppendMode.Append)
	        {
                ILProcessor.Append(instruction);
            }
            else if (AppendMode == AppendMode.Insert)
	        {
	            if (LastEmittedInstruction == null)
	            {
	                ILProcessor.InsertBefore(Body.Instructions[0], instruction);
	            }
	            else
	            {
                    ILProcessor.InsertAfter(LastEmittedInstruction, instruction);
                }
            }
	    }

		public FluentEmitter Emit(Instruction instruction)
		{
		    EmitAction(instruction);

			LastEmittedInstruction = instruction;
			
			while (PostEmitActions.Count != 0)
			{
				var action = PostEmitActions.Dequeue();
				if(!action(this))
					PostEmitActions.Enqueue(action);
			}

		    if (StackValidationMode == StackValidationMode.Manual
                || StackValidationMode == StackValidationMode.OnReturn && instruction.OpCode != OpCodes.Ret)
                return this;

            //var validator = new FlowControlAnalyzer(Body);
		    //validator.ValidateFullStackOrThrow();

		    return this;
		}

		public FluentEmitter Emit(OpCode opcode)
		{
			return Emit(Instruction.Create(opcode));
		}
		
		public FluentEmitter Emit(OpCode opcode, string arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentEmitter Emit(OpCode opcode, int arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}
		
		public FluentEmitter Emit(OpCode opcode, sbyte arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}
		
		public FluentEmitter Emit(OpCode opcode, long arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentEmitter Emit(OpCode opcode, float arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentEmitter Emit(OpCode opcode, double arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentEmitter Emit(OpCode opcode, MethodInfo arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentEmitter Emit(OpCode opcode, ConstructorInfo arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentEmitter Emit(OpCode opcode, FieldInfo arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentEmitter Emit(OpCode opcode, SystemTypeOrTypeReference arg)
		{
			return Emit(Instruction.Create(opcode, arg.GetTypeReference(Module)));
		}

		public FluentEmitter Emit(OpCode opcode, FieldReference arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentEmitter Emit(OpCode opcode, MethodReference arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentEmitter Emit(OpCode opcode, VariableDefinition arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentEmitter Emit(OpCode opcode, ParameterDefinition arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentEmitter Emit(OpCode opcode, Instruction arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentEmitter Emit(OpCode opcode, Func<Collection<Instruction>, Instruction> selector)
		{
			return Emit(opcode, selector(Body.Instructions));
		}
	}
}
