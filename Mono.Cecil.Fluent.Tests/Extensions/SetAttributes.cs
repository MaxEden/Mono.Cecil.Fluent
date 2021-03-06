﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil.Fluent.Attributes;
using Mono.Cecil.Fluent.Utils;
using Should.Fluent;

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace Mono.Cecil.Fluent.Tests.Extensions
{
    [TestClass]
	public class Extensions_SetAttributes : TestsBase
	{
		static readonly TypeDefinition TestType = CreateType();
		static readonly MethodDefinition TestMethod = CreateMethod();

		static PropertyDefinition CreateProperty()
		{
			var prop = new PropertyDefinition(Generate.Name.ForMethod(), PropertyAttributes.None, TestModule.TypeSystem.Boolean);
			TestType.Properties.Add(prop);
			prop.DeclaringType = TestType;
			return prop;
		}

        [TestMethod]
		public void set_attributes_for_method () =>
			TestMethod
				.SetMethodAttributes(MethodAttributes.Abstract, MethodAttributes.Final)
				.Attributes.Should().Equal(MethodAttributes.Family | MethodAttributes.Abstract | MethodAttributes.Final);

        [TestMethod]
        public void set_attributes_for_type () =>
			TestType
				.SetTypeAttributes(TypeAttributes.Abstract, TypeAttributes.Class)
				.Attributes.Should().Equal(TypeAttributes.Abstract | TypeAttributes.Class);

        [TestMethod]
        public void set_attributes_for_event () =>
			CreateEvent()
				.SetEventAttributes(EventAttributes.SpecialName | EventAttributes.RTSpecialName)
				.Attributes.Should().Equal(EventAttributes.SpecialName | EventAttributes.RTSpecialName);

        [TestMethod]
        public void set_attributes_for_property () =>
			CreateProperty()
				.SetPropertyAttributes(PropertyAttributes.RTSpecialName | PropertyAttributes.SpecialName)
				.Attributes.Should().Equal(PropertyAttributes.RTSpecialName | PropertyAttributes.SpecialName);

        [TestMethod]
        public void set_attributes_for_field () =>
			CreateField()
				.SetFieldAttributes(FieldAttributes.Assembly | FieldAttributes.Family)
				.Attributes.Should().Equal(FieldAttributes.Assembly | FieldAttributes.Family);

        [TestMethod]
        public void set_attributes_for_method_generic_part1 () =>
			TestMethod
				.UnsetAllMethodAttributes()
				.SetMethodAttributes<MemberAccessMask, Private, FamANDAssem, HideBySig, NewSlot, Public, Final, Virtual>()
				.Attributes.Should().Equal(MethodAttributes.MemberAccessMask | MethodAttributes.Private | MethodAttributes.FamANDAssem | MethodAttributes.HideBySig | MethodAttributes.NewSlot 
					| MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual);

        [TestMethod]
        public void set_attributes_for_method_generic_part2 () =>
			TestMethod
				.UnsetAllMethodAttributes()
				.SetMethodAttributes<Assembly, Family, Static, VtableLayoutMask, CheckAccessOnOverride, Abstract, SpecialName, PInvokeImpl>()
				.Attributes.Should().Equal(MethodAttributes.Assembly | MethodAttributes.Static | MethodAttributes.VtableLayoutMask | MethodAttributes.CheckAccessOnOverride
				| MethodAttributes.Family | MethodAttributes.Abstract | MethodAttributes.SpecialName | MethodAttributes.PInvokeImpl);

        [TestMethod]
        public void set_attributes_for_method_generic_part3 () =>
			TestMethod
				.UnsetAllMethodAttributes()
				.SetMethodAttributes<FamORAssem, UnmanagedExport, RTSpecialName, HasSecurity, RequireSecObject, ReuseSlot, CompilerControlled>() // last two are 0
				.Attributes.Should().Equal(MethodAttributes.FamORAssem | MethodAttributes.UnmanagedExport | MethodAttributes.RTSpecialName | MethodAttributes.HasSecurity | MethodAttributes.RequireSecObject);

        // improve code coverage:

        [TestMethod]
        public void set_attributes_for_method_generic_part4 () =>
			TestMethod
				.UnsetAllMethodAttributes()
				.SetMethodAttributes<MemberAccessMask, Private, FamANDAssem, HideBySig, NewSlot, Public, Final, Virtual>()
				.Attributes.Should().Equal(MethodAttributes.MemberAccessMask | MethodAttributes.Private | MethodAttributes.FamANDAssem | MethodAttributes.HideBySig | MethodAttributes.NewSlot
					| MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual);

        [TestMethod]
        public void set_attributes_for_type_generic_part1 () =>
			TestType
				.UnsetAllTypeAttributes()
				.SetTypeAttributes<VisibilityMask, Public, NestedPublic, NestedFamily, LayoutMask, AutoLayout, SequentialLayout, ExplicitLayout>()
				.Attributes.Should().Equal(TypeAttributes.VisibilityMask | TypeAttributes.Public | TypeAttributes.NestedPublic | TypeAttributes.NestedFamily | TypeAttributes.LayoutMask
					| TypeAttributes.AutoLayout | TypeAttributes.SequentialLayout | TypeAttributes.ExplicitLayout);

        [TestMethod]
        public void set_attributes_for_type_generic_part2 () =>
			TestType
				.UnsetAllTypeAttributes()
				.SetTypeAttributes<NestedPrivate, ClassSemanticMask, Interface, Abstract, Sealed, SpecialName, Import, Serializable>()
				.Attributes.Should().Equal(TypeAttributes.NestedPrivate | TypeAttributes.ClassSemanticMask | TypeAttributes.Interface | TypeAttributes.Abstract
					| TypeAttributes.Sealed | TypeAttributes.SpecialName | TypeAttributes.Import | TypeAttributes.Serializable);

        [TestMethod]
        public void set_attributes_for_type_generic_part3 () =>
			TestType
				.UnsetAllTypeAttributes()
				.SetTypeAttributes<WindowsRuntime, NestedAssembly, StringFormatMask, AnsiClass, UnicodeClass, AutoClass, BeforeFieldInit, RTSpecialName>()
				.Attributes.Should().Equal(TypeAttributes.WindowsRuntime | TypeAttributes.NestedAssembly | TypeAttributes.StringFormatMask | TypeAttributes.AnsiClass
					| TypeAttributes.UnicodeClass | TypeAttributes.AutoClass | TypeAttributes.BeforeFieldInit | TypeAttributes.RTSpecialName);

        [TestMethod]
        public void set_attributes_for_type_generic_part4 () =>
			TestType
				.UnsetAllTypeAttributes()
				.SetTypeAttributes<NestedFamANDAssem, NestedFamORAssem, HasSecurity, Forwarder, Class, NotPublic>() // last two are 0
				.Attributes.Should().Equal(TypeAttributes.NestedFamANDAssem | TypeAttributes.NestedFamORAssem | TypeAttributes.HasSecurity | TypeAttributes.Forwarder);

        [TestMethod]
        public void set_attributes_for_event_generic () =>
			CreateEvent()
				.SetEventAttributes<SpecialName, RTSpecialName>()
				.Attributes.Should().Equal(EventAttributes.SpecialName | EventAttributes.RTSpecialName);

        [TestMethod]
        public void set_attributes_for_property_generic () =>
			CreateProperty()
				.SetPropertyAttributes<SpecialName, RTSpecialName, HasDefault> ()
				.Attributes.Should().Equal(PropertyAttributes.SpecialName | PropertyAttributes.RTSpecialName | PropertyAttributes.HasDefault);

        [TestMethod]
        public void set_attributes_for_field_generic_part1 () =>
			CreateField()
				.SetFieldAttributes<FieldAccessMask, Private, FamANDAssem, Family, Static, InitOnly, Literal, NotSerialized>()
				.Attributes.Should().Equal(FieldAttributes.FieldAccessMask | FieldAttributes.Private | FieldAttributes.FamANDAssem | FieldAttributes.Family
					| FieldAttributes.Static | FieldAttributes.InitOnly | FieldAttributes.Literal | FieldAttributes.NotSerialized);

        [TestMethod]
        public void set_attributes_for_field_generic_part2 () =>
			CreateField()
				.SetFieldAttributes<Assembly, FamORAssem, SpecialName, PInvokeImpl, RTSpecialName, HasFieldMarshal, HasDefault, HasFieldRVA>()
				.Attributes.Should().Equal(FieldAttributes.Assembly | FieldAttributes.FamORAssem | FieldAttributes.SpecialName | FieldAttributes.PInvokeImpl
					| FieldAttributes.RTSpecialName | FieldAttributes.HasFieldMarshal | FieldAttributes.HasDefault | FieldAttributes.HasFieldRVA);

        [TestMethod]
        public void set_attributes_for_field_generic_part3 () =>
			CreateField()
				.SetFieldAttributes<Public, CompilerControlled>() // last one is 0
				.Attributes.Should().Equal(FieldAttributes.Public);
	}
}
