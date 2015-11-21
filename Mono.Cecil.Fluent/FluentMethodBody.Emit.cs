﻿using System;
using System.Reflection;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using OpCode = Mono.Cecil.Cil.OpCode;

namespace Mono.Cecil.Fluent
{
	partial class FluentMethodBody
	{
		private Action<Instruction> _emitAction;

		public FluentMethodBody Emit(Instruction instruction)
		{
			// it is just covered by all other emit() methods
			//ncrunch: no coverage start
			if (_emitAction == null)
				_emitAction = i => MethodDefinition.Body.Instructions.Add(i);

			_emitAction(instruction);
			return this;
			//ncrunch: no coverage end
		}

		public FluentMethodBody Emit(OpCode opcode)
		{
			return Emit(Instruction.Create(opcode));
		}
		
		public FluentMethodBody Emit(OpCode opcode, string arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentMethodBody Emit(OpCode opcode, int arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}
		
		public FluentMethodBody Emit(OpCode opcode, sbyte arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}
		
		public FluentMethodBody Emit(OpCode opcode, long arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentMethodBody Emit(OpCode opcode, float arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentMethodBody Emit(OpCode opcode, double arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentMethodBody Emit(OpCode opcode, MethodInfo arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, ConstructorInfo arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, FieldInfo arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, Type arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, TypeInfo arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, TypeReference arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, FieldReference arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, MethodReference arg)
		{
			return Emit(Instruction.Create(opcode, Module.SafeImport(arg)));
		}

		public FluentMethodBody Emit(OpCode opcode, VariableDefinition arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentMethodBody Emit(OpCode opcode, Instruction arg)
		{
			return Emit(Instruction.Create(opcode, arg));
		}

		public FluentMethodBody Emit(OpCode opcode, Func<Collection<Instruction>, Instruction> selector)
		{
			return Emit(opcode, selector(Body.Instructions));
		}
	}
}
