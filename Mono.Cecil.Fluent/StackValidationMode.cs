﻿namespace Mono.Cecil.Fluent
{
    public enum StackValidationMode
    {
        /// <summary>
        /// Validates stack after emitting an Instruction
        /// </summary>
        OnEmit,
        /// <summary>
        /// Validates stack after emitting an return instruction
        /// </summary>
        OnReturn,
        /// <summary>
        /// Disables automatic stack validation. You have to call ValidateStack() manually.
        /// </summary>
        Manual
    }
}
