﻿using System;
using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
	internal sealed class MagicNumberArgumentR4 : MagicNumberArgument
	{
		internal readonly float Number;

		internal override bool IsZero => float.IsNaN(Number) || System.Math.Abs(Number) < float.Epsilon;

		internal MagicNumberArgumentR4(float num) : base(false, true, false)
		{
			Number = num;
		}

		internal override FluentEmitter EmitLdc(FluentEmitter method)
		{
			return method.Emit(OpCodes.Ldc_R4, Number);
		}

		internal override FluentEmitter EmitLdcI4(FluentEmitter method)
		{
			return new MagicNumberArgumentI4((int) Number).EmitLdc(method);
		}

		internal override FluentEmitter EmitLdcI8(FluentEmitter method)
		{
			return new MagicNumberArgumentI8((long) Number).EmitLdc(method);
		}

		internal override FluentEmitter EmitLdcR4(FluentEmitter method)
		{
			return EmitLdc(method);
		}

		internal override FluentEmitter EmitLdcR8(FluentEmitter method)
		{
			return new MagicNumberArgumentR8(Number).EmitLdc(method);
		}
	}
}
