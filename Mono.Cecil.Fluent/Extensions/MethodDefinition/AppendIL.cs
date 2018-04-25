
// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace

using System;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Mono.Cecil.Fluent
{
    partial class MethodDefinitionExtensions
    {
        /// <summary>
        /// Compilation of MethodDefinition and bind to given to Func or Action signature.
        /// </summary>
		public static FluentEmitter AppendIL(this MethodDefinition method)
        {
            return new FluentEmitter(method, AppendMode.Append, null);
        }

        public static FluentEmitter InsertILBefore(this MethodDefinition method, Func<Collection<Instruction>, Instruction> instructionSelector)
        {
            return InsertILBefore(method, instructionSelector(method.Body.Instructions));
        }

        public static FluentEmitter InsertILAfter(this MethodDefinition method, Func<Collection<Instruction>, Instruction> instructionSelector)
        {
            return InsertILAfter(method, instructionSelector(method.Body.Instructions));
        }

        public static FluentEmitter InsertILAfter(this MethodDefinition method, Instruction instruction)
        {
            return new FluentEmitter(method, AppendMode.Insert, instruction);
        }

        public static FluentEmitter InsertILBefore(this MethodDefinition method, Instruction instruction)
        {
            return new FluentEmitter(method, AppendMode.Insert, instruction.Previous);
        }

        public static FluentEmitter InsertILTail(this MethodDefinition method)
        {
            return InsertILBefore(method, method.LastRet());
        }

        public static FluentEmitter InsertILHead(this MethodDefinition method)
        {
            return InsertILBefore(method, p=> p[0]);
        }
    }

    public enum AppendMode
    {
        Append,
        Insert
    }
}
