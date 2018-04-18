﻿// Copyright (c) 2011 AlphaSierraPapa for the SharpDevelop Team
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

// ReSharper disable CheckNamespace
namespace ICSharpCode.Decompiler.Disassembler
{
	internal enum IlNameSyntax
	{
		/// <summary>
		/// class/valuetype + TypeName (built-in types use keyword syntax)
		/// </summary>
		Signature,
		/// <summary>
		/// Like signature, but always refers to type parameters using their position
		/// </summary>
		SignatureNoNamedTypeParameters,
		/// <summary>
		/// [assembly]Full.Type.Name (even for built-in types)
		/// </summary>
		TypeName,
		/// <summary>
		/// Name (but built-in types use keyword syntax)
		/// </summary>
		ShortTypeName
	}

	internal static class DisassemblerHelpers
	{
		public static void WriteOffsetReference(PlainTextOutput writer, Instruction instruction)
		{
			writer.Write($"IL_{instruction.Offset:x4}");
		}

		public static void WriteTo(this Instruction instruction, PlainTextOutput writer)
		{
			writer.Write($"IL_{instruction.Offset:x4}");
			writer.Write(": ");
			writer.Write(instruction.OpCode.Name);

			if (instruction.Operand == null)
				return;

			writer.Write(' ');
			if (instruction.OpCode == OpCodes.Ldtoken)
			{
				if (instruction.Operand is MethodReference)
					writer.Write("method ");
				else if (instruction.Operand is FieldReference)
					writer.Write("field ");
			}
			WriteOperand(writer, instruction.Operand);
		}

		private static void WriteLabelList(PlainTextOutput writer, Instruction[] instructions)
		{
			writer.Write("(");
			for (var i = 0; i < instructions.Length; i++)
			{
				if (i != 0) writer.Write(", ");
				WriteOffsetReference(writer, instructions[i]);
			}
			writer.Write(")");
		}

		private static string ToInvariantCultureString(object value)
		{
			var convertible = value as IConvertible;
			return convertible?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? value.ToString();
		}

		public static void WriteTo(this MethodReference method, PlainTextOutput writer)
		{
			if (method.ExplicitThis)
				writer.Write("instance explicit ");
			else if (method.HasThis)
				writer.Write("instance ");
			method.ReturnType.WriteTo(writer, IlNameSyntax.SignatureNoNamedTypeParameters);
			writer.Write(' ');
			if (method.DeclaringType != null)
			{
				method.DeclaringType.WriteTo(writer, IlNameSyntax.TypeName);
				writer.Write("::");
			}
			var md = method as MethodDefinition;
			if (md != null && md.IsCompilerControlled)
			{
				writer.Write(Escape(method.Name + "$PST" + method.MetadataToken.ToInt32().ToString("X8")));
			}
			else
			{
				writer.Write(Escape(method.Name));
			}
			var gim = method as GenericInstanceMethod;
			if (gim != null)
			{
				writer.Write('<');
				for (var i = 0; i < gim.GenericArguments.Count; i++)
				{
					if (i > 0)
						writer.Write(", ");
					gim.GenericArguments[i].WriteTo(writer);
				}
				writer.Write('>');
			}
			writer.Write("(");
			var parameters = method.Parameters;
			for (var i = 0; i < parameters.Count; ++i)
			{
				if (i > 0) writer.Write(", ");
				parameters[i].ParameterType.WriteTo(writer, IlNameSyntax.SignatureNoNamedTypeParameters);
			}
			writer.Write(")");
		}

		private static void WriteTo(this FieldReference field, PlainTextOutput writer)
		{
			field.FieldType.WriteTo(writer, IlNameSyntax.SignatureNoNamedTypeParameters);
			writer.Write(' ');
			field.DeclaringType.WriteTo(writer, IlNameSyntax.TypeName);
			writer.Write("::");
			writer.Write(Escape(field.Name));
		}

		private static bool IsValidIdentifierCharacter(char c)
		{
			return c == '_' || c == '$' || c == '@' || c == '?' || c == '`';
		}

		private static bool IsValidIdentifier(string identifier)
		{
			if (string.IsNullOrEmpty(identifier))
				return false;
			if (!(char.IsLetter(identifier[0]) || IsValidIdentifierCharacter(identifier[0])))
			{
				// As a special case, .ctor and .cctor are valid despite starting with a dot
				return identifier == ".ctor" || identifier == ".cctor";
			}
			for (var i = 1; i < identifier.Length; i++)
			{
				if (!(char.IsLetterOrDigit(identifier[i]) || IsValidIdentifierCharacter(identifier[i]) || identifier[i] == '.'))
					return false;
			}
			return true;
		}

		private static readonly HashSet<string> IlKeywords = BuildKeywordList(
		"abstract", "algorithm", "alignment", "ansi", "any", "arglist",
		"array", "as", "assembly", "assert", "at", "auto", "autochar", "beforefieldinit",
		"blob", "blob_object", "bool", "brnull", "brnull.s", "brzero", "brzero.s", "bstr",
		"bytearray", "byvalstr", "callmostderived", "carray", "catch", "cdecl", "cf",
		"char", "cil", "class", "clsid", "const", "currency", "custom", "date", "decimal",
		"default", "demand", "deny", "endmac", "enum", "error", "explicit", "extends", "extern",
		"false", "famandassem", "family", "famorassem", "fastcall", "fault", "field", "filetime",
		"filter", "final", "finally", "fixed", "float", "float32", "float64", "forwardref",
		"fromunmanaged", "handler", "hidebysig", "hresult", "idispatch", "il", "illegal",
		"implements", "implicitcom", "implicitres", "import", "in", "inheritcheck", "init",
		"initonly", "instance", "int", "int16", "int32", "int64", "int8", "interface", "internalcall",
		"iunknown", "lasterr", "lcid", "linkcheck", "literal", "localloc", "lpstr", "lpstruct", "lptstr",
		"lpvoid", "lpwstr", "managed", "marshal", "method", "modopt", "modreq", "native", "nested",
		"newslot", "noappdomain", "noinlining", "nomachine", "nomangle", "nometadata", "noncasdemand",
		"noncasinheritance", "noncaslinkdemand", "noprocess", "not", "not_in_gc_heap", "notremotable",
		"notserialized", "null", "nullref", "object", "objectref", "opt", "optil", "out",
		"permitonly", "pinned", "pinvokeimpl", "prefix1", "prefix2", "prefix3", "prefix4", "prefix5", "prefix6",
		"prefix7", "prefixref", "prejitdeny", "prejitgrant", "preservesig", "private", "privatescope", "protected",
		"public", "record", "refany", "reqmin", "reqopt", "reqrefuse", "reqsecobj", "request", "retval",
		"rtspecialname", "runtime", "safearray", "sealed", "sequential", "serializable", "special", "specialname",
		"static", "stdcall", "storage", "stored_object", "stream", "streamed_object", "string", "struct",
		"synchronized", "syschar", "sysstring", "tbstr", "thiscall", "tls", "to", "true", "typedref",
		"unicode", "unmanaged", "unmanagedexp", "unsigned", "unused", "userdefined", "value", "valuetype",
		"vararg", "variant", "vector", "virtual", "void", "wchar", "winapi", "with", "wrapper",
		// These are not listed as keywords in spec, but ILAsm treats them as such
		"property", "type", "flags", "callconv", "strict"
		);

		private static HashSet<string> BuildKeywordList(params string[] keywords)
		{
			var s = new HashSet<string>(keywords);
			foreach (var field in typeof(OpCodes).GetFields())
			{
				s.Add(((OpCode)field.GetValue(null)).Name);
			}
			return s;
		}

		public static string Escape(string identifier)
		{
			if (IsValidIdentifier(identifier) && !IlKeywords.Contains(identifier))
			{
				return identifier;
			}
			// The ECMA specification says that ' inside SQString should be ecaped using an octal escape sequence,
			// but we follow Microsoft's ILDasm and use \'.
			return "'" + ConvertString(identifier).Replace("'", "\\'") + "'";
		}

		public static void WriteTo(this TypeReference type, PlainTextOutput writer, IlNameSyntax syntax = IlNameSyntax.Signature)
		{
			var syntaxForElementTypes = syntax == IlNameSyntax.SignatureNoNamedTypeParameters ? syntax : IlNameSyntax.Signature;
			if (type is PinnedType)
			{
				((PinnedType)type).ElementType.WriteTo(writer, syntaxForElementTypes);
				writer.Write(" pinned");
			}
			else if (type is ArrayType)
			{
				var at = (ArrayType)type;
				at.ElementType.WriteTo(writer, syntaxForElementTypes);
				writer.Write('[');
				writer.Write(string.Join(", ", at.Dimensions.Select(p=>p.ToString()).ToArray()));
				writer.Write(']');
			}
			else if (type is GenericParameter)
			{
				writer.Write('!');
				if (((GenericParameter)type).Owner.GenericParameterType == GenericParameterType.Method)
					writer.Write('!');
				if (string.IsNullOrEmpty(type.Name) || type.Name[0] == '!' || syntax == IlNameSyntax.SignatureNoNamedTypeParameters)
					writer.Write(((GenericParameter)type).Position.ToString());
				else
					writer.Write(Escape(type.Name));
			}
			else if (type is ByReferenceType)
			{
				((ByReferenceType)type).ElementType.WriteTo(writer, syntaxForElementTypes);
				writer.Write('&');
			}
			else if (type is PointerType)
			{
				((PointerType)type).ElementType.WriteTo(writer, syntaxForElementTypes);
				writer.Write('*');
			}
			else if (type is GenericInstanceType)
			{
				type.GetElementType().WriteTo(writer, syntaxForElementTypes);
				writer.Write('<');
				var arguments = ((GenericInstanceType)type).GenericArguments;
				for (var i = 0; i < arguments.Count; i++)
				{
					if (i > 0)
						writer.Write(", ");
					arguments[i].WriteTo(writer, syntaxForElementTypes);
				}
				writer.Write('>');
			}
			else if (type is OptionalModifierType)
			{
				((OptionalModifierType)type).ElementType.WriteTo(writer, syntax);
				writer.Write(" modopt(");
				((OptionalModifierType)type).ModifierType.WriteTo(writer, IlNameSyntax.TypeName);
				writer.Write(") ");
			}
			else if (type is RequiredModifierType)
			{
				((RequiredModifierType)type).ElementType.WriteTo(writer, syntax);
				writer.Write(" modreq(");
				((RequiredModifierType)type).ModifierType.WriteTo(writer, IlNameSyntax.TypeName);
				writer.Write(") ");
			}
			else
			{
				var name = PrimitiveTypeName(type.FullName);
				if ((syntax == IlNameSyntax.Signature || syntax == IlNameSyntax.SignatureNoNamedTypeParameters) && name != null)
				{
					writer.Write(name);
				}
				else
				{
					if (syntax == IlNameSyntax.Signature || syntax == IlNameSyntax.SignatureNoNamedTypeParameters)
						writer.Write(type.IsValueType ? "valuetype " : "class ");
					if (type.DeclaringType != null)
					{
						type.DeclaringType.WriteTo(writer, IlNameSyntax.TypeName);
						writer.Write('/');
						writer.Write(Escape(type.Name));
					}
					else
					{
						if (!type.IsDefinition && type.Scope != null && !(type is TypeSpecification))
							writer.Write("[{0}]", Escape(type.Scope.Name));
						writer.Write(Escape(type.FullName));
					}
				}
			}
		}

		public static void WriteOperand(PlainTextOutput writer, object operand)
		{
			if (operand == null)
				throw new ArgumentNullException(nameof(operand));
		    if (operand is Instruction targetInstruction)
			{
				WriteOffsetReference(writer, targetInstruction);
				return;
			}

		    if (operand is Instruction[] targetInstructions)
			{
				WriteLabelList(writer, targetInstructions);
				return;
			}

		    if (operand is VariableReference variableRef)
			{

                writer.Write(variableRef.Index.ToString());
			    return;
			}

		    if (operand is ParameterReference paramRef)
			{
			    writer.Write(string.IsNullOrEmpty(paramRef.Name) ? paramRef.Index.ToString() : Escape(paramRef.Name));
			    return;
			}

		    if (operand is MethodReference methodRef)
			{
				methodRef.WriteTo(writer);
				return;
			}

		    if (operand is TypeReference typeRef)
			{
				typeRef.WriteTo(writer, IlNameSyntax.TypeName);
				return;
			}

		    if (operand is FieldReference fieldRef)
			{
				fieldRef.WriteTo(writer);
				return;
			}

		    if (operand is string s)
			{
				writer.Write("\"" + ConvertString(s) + "\"");
			}
			else if (operand is char)
			{
				writer.Write(((int)(char)operand).ToString());
			}
			else if (operand is float)
			{
				var val = (float)operand;
				if (val == 0)
				{
					if (float.IsNegativeInfinity(1 / val))
						// negative zero is a special case
						writer.Write('-');
					writer.Write("0.0");
				}
				else if (float.IsInfinity(val) || float.IsNaN(val))
				{
					var data = BitConverter.GetBytes(val);
					writer.Write('(');
					for (var i = 0; i < data.Length; i++)
					{
						if (i > 0)
							writer.Write(' ');
						writer.Write(data[i].ToString("X2"));
					}
					writer.Write(')');
				}
				else
				{
					writer.Write(val.ToString("R", System.Globalization.CultureInfo.InvariantCulture));
				}
			}
			else if (operand is double)
			{
				var val = (double)operand;
				if (val == 0)
				{
					if (double.IsNegativeInfinity(1 / val))
						// negative zero is a special case
						writer.Write('-');
					writer.Write("0.0");
				}
				else if (double.IsInfinity(val) || double.IsNaN(val))
				{
					var data = BitConverter.GetBytes(val);
					writer.Write('(');
					for (var i = 0; i < data.Length; i++)
					{
						if (i > 0)
							writer.Write(' ');
						writer.Write(data[i].ToString("X2"));
					}
					writer.Write(')');
				}
				else
				{
					writer.Write(val.ToString("R", System.Globalization.CultureInfo.InvariantCulture));
				}
			}
			else if (operand is bool)
			{
				writer.Write((bool)operand ? "true" : "false");
			}
			else
			{
				s = ToInvariantCultureString(operand);
				writer.Write(s);
			}
		}

		public static string PrimitiveTypeName(string fullName)
		{
			switch (fullName)
			{
				case "System.SByte":
					return "int8";
				case "System.Int16":
					return "int16";
				case "System.Int32":
					return "int32";
				case "System.Int64":
					return "int64";
				case "System.Byte":
					return "uint8";
				case "System.UInt16":
					return "uint16";
				case "System.UInt32":
					return "uint32";
				case "System.UInt64":
					return "uint64";
				case "System.Single":
					return "float32";
				case "System.Double":
					return "float64";
				case "System.Void":
					return "void";
				case "System.Boolean":
					return "bool";
				case "System.String":
					return "string";
				case "System.Char":
					return "char";
				case "System.Object":
					return "object";
				case "System.IntPtr":
					return "native int";
				default:
					return null;
			}
		}

		/// <summary>
		/// Converts special characters to escape sequences within the given string. 
		/// </summary>
		public static string ConvertString(string str)
		{
			var stringBuilder = new StringBuilder();
			foreach (var ch in str)
			    stringBuilder.Append((int) ch == 34 ? "\\\"" : ConvertChar(ch));

			return stringBuilder.ToString();
		}
		
		/// <summary>
		/// Gets the escape sequence for the specified character. This method does not convert ' or ".
		/// </summary>
		private static string ConvertChar(char ch)
		{
			switch (ch)
			{
				//ncrunch: no coverage start
				case char.MinValue:
					return "\\0";
				case '\a':
					return "\\a";
				case '\b':
					return "\\b";
				case '\t':
					return "\\t";
				case '\n':
					return "\\n";
				case '\v':
					return "\\v";
				case '\f':
					return "\\f";
				case '\r':
					return "\\r";
				case '\\':
					return "\\\\";
				//ncrunch: no coverage end
				default:
					if (char.IsControl(ch) || char.IsSurrogate(ch) || char.IsWhiteSpace(ch) && ch != 32)
						return "\\u" + ((int)ch).ToString("x4");
					return ch.ToString();
			}
		}
	}
}