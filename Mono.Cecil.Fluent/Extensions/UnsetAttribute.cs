﻿using System;
using System.Runtime.InteropServices;

namespace Mono.Cecil.Fluent
{
	public static partial class FieldDefinitionExtensions
	{
		public static FieldDefinition UnsetAttributes(this FieldDefinition field, params FieldAttributes[] attributes)
		{
			foreach (var attribute in attributes)
				field.Attributes &= ~attribute;
			return field;
		}

		public static FieldDefinition UnsetAllAttributes(this FieldDefinition field)
		{
			field.Attributes = 0;
			return field;
		}
	}

	public static partial class PropertyDefinitionExtensions
	{
		public static PropertyDefinition UnsetAttributes(this PropertyDefinition property, params PropertyAttributes[] attributes)
		{
			foreach (var attribute in attributes)
				property.Attributes &= ~attribute;
			return property;
		}
		public static PropertyDefinition UnsetAllAttributes(this PropertyDefinition property)
		{
			property.Attributes = 0;
			return property;
		}
	}

	public static partial class EventDefinitionExtensions
	{
		public static EventDefinition UnsetAttributes(this EventDefinition @event, params EventAttributes[] attributes)
		{
			foreach (var attribute in attributes)
				@event.Attributes &= ~attribute;
			return @event;
		}
		public static EventDefinition UnsetAllAttributes(this EventDefinition @event)
		{
			@event.Attributes = 0;
			return @event;
		}
	}

	public static partial class TypeDefinitionExtensions
	{
		public static TypeDefinition UnsetAttributes(this TypeDefinition type, params TypeAttributes[] attributes)
		{
			foreach (var attribute in attributes)
				type.Attributes &= ~attribute;
			return type;
		}
		public static TypeDefinition UnsetAllAttributes(this TypeDefinition type)
		{
			type.Attributes = 0;
			return type;
		}
	}

	public static partial class MethodDefinitionExtensions
	{
		public static FluentMethodBody UnsetAttributes(this MethodDefinition method, params MethodAttributes[] attributes)
		{
			return new FluentMethodBody(method).UnsetAttributes(attributes);
		}
		public static FluentMethodBody UnsetAllAttributes(this MethodDefinition method)
		{
			return new FluentMethodBody(method).UnsetAllAttributes();
		}
	}

	public static partial class FluentMethodBodyExtensions
	{
		public static FluentMethodBody UnsetAttributes(this FluentMethodBody method, params MethodAttributes[] attributes)
		{
			foreach (var attribute in attributes)
				method.Attributes &= ~attribute;
			return method;
		}
		public static FluentMethodBody UnsetAllAttributes(this FluentMethodBody method)
		{
			method.Attributes = 0;
			return method;
		}
	}
}
