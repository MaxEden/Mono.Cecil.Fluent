using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
    internal class IfBlock
	{
		public Instruction StartInstruction;
		public bool IsDoublePop;
		public OpCode OpCode;
	}

	partial class FluentEmitter
	{
		internal readonly Stack<IfBlock> IfBlocks = new Stack<IfBlock>();

		public FluentEmitter IfTrue()
		{
			Pop();
			var block = new IfBlock() { StartInstruction = LastEmittedInstruction, OpCode = OpCodes.Brfalse }; // it jumps if value top on stack is false
			IfBlocks.Push(block);
			return this;
		}

		public FluentEmitter IfNot()
		{
			Pop();
			var block = new IfBlock() { StartInstruction = LastEmittedInstruction, OpCode = OpCodes.Brtrue }; // it jumps if value top on stack is true
			IfBlocks.Push(block);
			return this;
		}

		public FluentEmitter Iflt()
		{
			Pop();
			var pop1 = LastEmittedInstruction;
			Pop();
			var block = new IfBlock() { StartInstruction = pop1, IsDoublePop = true, OpCode = OpCodes.Bge };
			IfBlocks.Push(block);
			return this;
		}

		public FluentEmitter Ifgt()
		{
			Pop();
			var pop1 = LastEmittedInstruction;
			Pop();
			var block = new IfBlock() { StartInstruction = pop1, IsDoublePop = true, OpCode = OpCodes.Ble };
			IfBlocks.Push(block);
			return this;
		}

		public FluentEmitter Iflte()
		{
			Pop();
			var pop1 = LastEmittedInstruction;
			Pop();
			var block = new IfBlock() { StartInstruction = pop1, IsDoublePop = true, OpCode = OpCodes.Bgt };
			IfBlocks.Push(block);
			return this;
		}

		public FluentEmitter Ifgte()
		{
			Pop();
			var pop1 = LastEmittedInstruction;
			Pop();
			var block = new IfBlock() { StartInstruction = pop1, IsDoublePop = true, OpCode = OpCodes.Blt };
			IfBlocks.Push(block);
			return this;
		}

		public FluentEmitter Else()
		{
			if(IfBlocks.Count == 0 || IfBlocks.Peek().OpCode == OpCodes.Br)
				throw new Exception("no if blfor else");
			Nop();
			var jumpstart = LastEmittedInstruction;
			EndIf();
			var block = new IfBlock() { StartInstruction = jumpstart, OpCode = OpCodes.Br }; // it jumps if value top on stack is true
			IfBlocks.Push(block);
			return this;
		}

		public FluentEmitter EndIf()
		{
			if(IfBlocks.Count == 0)
				throw new NotSupportedException("no if-block to close");

			var block = IfBlocks.Pop();

			if (block.StartInstruction?.Previous.OpCode == OpCodes.Ret)
			{
				Body.Instructions.Remove(block.StartInstruction);
				return this;
			}

		    Nop();
			var firstInstructionAfterBlock = LastEmittedInstruction;

			if(block.IsDoublePop)
				Body.GetILProcessor().Remove(block.StartInstruction?.Next);
            
			var newstartinstruction = Instruction.Create(block.OpCode, firstInstructionAfterBlock);
			Body.GetILProcessor().Replace(block.StartInstruction, newstartinstruction);

			// remove Nops
			Func<FluentEmitter, bool> postemitaction = (body) =>
			{
				if (firstInstructionAfterBlock.Next == null)
					return false;
				foreach (var instruction in body.Body.Instructions.Where(i => i.Operand == firstInstructionAfterBlock))
					instruction.Operand = firstInstructionAfterBlock.Next;
				Body.GetILProcessor().Remove(firstInstructionAfterBlock);
				return true;
			};

			PostEmitActions.Enqueue(postemitaction);
			
			return this;
		}
	}
}