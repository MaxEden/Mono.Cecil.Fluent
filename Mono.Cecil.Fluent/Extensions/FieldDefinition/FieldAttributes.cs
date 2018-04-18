﻿
// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent.Attributes
{
    public interface IFieldAttribute
    {
        FieldAttributes FieldAttributesValue { get; }
    }

    public static class FieldDefinitionExtensions
    {
        public static FieldDefinition UnsetFieldAttributes(this FieldDefinition field, params FieldAttributes[] attributes)
        {
            foreach (var attribute in attributes)
                field.Attributes &= ~attribute;
            return field;
        }

        public static FieldDefinition UnsetAllFieldAttributes(this FieldDefinition field)
        {
            field.Attributes = 0;
            return field;
        }

		public static FieldDefinition SetFieldAttributes(this FieldDefinition field, params FieldAttributes[] attributes)
		{
			foreach (var attribute in attributes)
				field.Attributes |= attribute;
			return field;
		}
		public static FieldDefinition SetFieldAttributes<TAttr>(this FieldDefinition field) 
			where TAttr : struct, IFieldAttribute
		{
			field.Attributes |= default(TAttr).FieldAttributesValue;
			return field;
		}
		public static FieldDefinition SetFieldAttributes<TAttr1, TAttr2>(this FieldDefinition field)
			where TAttr1 : struct, IFieldAttribute
			where TAttr2 : struct, IFieldAttribute
		{
			return field.SetFieldAttributes<TAttr1>()
				.SetFieldAttributes<TAttr2>();
		}
		public static FieldDefinition SetFieldAttributes<TAttr1, TAttr2, TAttr3>(this FieldDefinition field)
			where TAttr1 : struct, IFieldAttribute
			where TAttr2 : struct, IFieldAttribute
			where TAttr3 : struct, IFieldAttribute
		{
			return field.SetFieldAttributes<TAttr1>()
				.SetFieldAttributes<TAttr2, TAttr3>();
		}
		public static FieldDefinition SetFieldAttributes<TAttr1, TAttr2, TAttr3, TAttr4>(this FieldDefinition field) 
			where TAttr1 : struct, IFieldAttribute
			where TAttr2 : struct, IFieldAttribute
			where TAttr3 : struct, IFieldAttribute
			where TAttr4 : struct, IFieldAttribute
		{
			return field.SetFieldAttributes<TAttr1>()
				.SetFieldAttributes<TAttr2, TAttr3, TAttr4>();
		}
		public static FieldDefinition SetFieldAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5>(this FieldDefinition field)
			where TAttr1 : struct, IFieldAttribute
			where TAttr2 : struct, IFieldAttribute
			where TAttr3 : struct, IFieldAttribute
			where TAttr4 : struct, IFieldAttribute
			where TAttr5 : struct, IFieldAttribute
		{
			return field.SetFieldAttributes<TAttr1>()
				.SetFieldAttributes<TAttr2, TAttr3, TAttr4, TAttr5>();
		}
		public static FieldDefinition SetFieldAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6>(this FieldDefinition field) 
			where TAttr1 : struct, IFieldAttribute
			where TAttr2 : struct, IFieldAttribute
			where TAttr3 : struct, IFieldAttribute
			where TAttr4 : struct, IFieldAttribute
			where TAttr5 : struct, IFieldAttribute
			where TAttr6 : struct, IFieldAttribute
		{
			return field.SetFieldAttributes<TAttr1>()
				.SetFieldAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6>();
		}
		public static FieldDefinition SetFieldAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7>(this FieldDefinition field)
			where TAttr1 : struct, IFieldAttribute
			where TAttr2 : struct, IFieldAttribute
			where TAttr3 : struct, IFieldAttribute
			where TAttr4 : struct, IFieldAttribute
			where TAttr5 : struct, IFieldAttribute
			where TAttr6 : struct, IFieldAttribute
			where TAttr7 : struct, IFieldAttribute
		{
			return field.SetFieldAttributes<TAttr1>()
				.SetFieldAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7>();
		}
		public static FieldDefinition SetFieldAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7, TAttr8>(this FieldDefinition field)
			where TAttr1 : struct, IFieldAttribute
			where TAttr2 : struct, IFieldAttribute
			where TAttr3 : struct, IFieldAttribute
			where TAttr4 : struct, IFieldAttribute
			where TAttr5 : struct, IFieldAttribute
			where TAttr6 : struct, IFieldAttribute
			where TAttr7 : struct, IFieldAttribute
			where TAttr8 : struct, IFieldAttribute
		{
			return field.SetFieldAttributes<TAttr1>()
				.SetFieldAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7, TAttr8>();
		}
	}
}
