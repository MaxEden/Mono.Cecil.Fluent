﻿
// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent.Attributes
{
    public interface IMethodAttribute
    {
        MethodAttributes MethodAttributesValue { get; }
    }

    public static partial class MethodDefinitionExtensions
    {
        public static MethodDefinition UnsetMethodAttributes(this   MethodDefinition   method,
                                                             params MethodAttributes[] attributes)
        {
            foreach (var attribute in attributes)
                method.Attributes &= ~attribute;
            return method;
        }

        public static MethodDefinition UnsetAllMethodAttributes(this MethodDefinition method)
        {
            method.Attributes = 0;
            return method;
        }

        public static MethodDefinition SetMethodAttributes(this   MethodDefinition   method,
                                                           params MethodAttributes[] attributes)
        {
            foreach (var attribute in attributes)
                method.Attributes |= attribute;
            return method;
        }

        public static MethodDefinition SetMethodAttributes<TAttr>(this MethodDefinition method)
            where TAttr : struct, IMethodAttribute
        {
            method.Attributes |= default(TAttr).MethodAttributesValue;
            return method;
        }

        public static MethodDefinition SetMethodAttributes<TAttr1, TAttr2>(this MethodDefinition method)
            where TAttr1 : struct, IMethodAttribute
            where TAttr2 : struct, IMethodAttribute
        {
            return method.SetMethodAttributes<TAttr1>()
                .SetMethodAttributes<TAttr2>();
        }

        public static MethodDefinition SetMethodAttributes<TAttr1, TAttr2, TAttr3>(this MethodDefinition method)
            where TAttr1 : struct, IMethodAttribute
            where TAttr2 : struct, IMethodAttribute
            where TAttr3 : struct, IMethodAttribute
        {
            return method.SetMethodAttributes<TAttr2, TAttr3>()
                .SetMethodAttributes<TAttr1>();
        }

        public static MethodDefinition SetMethodAttributes<TAttr1, TAttr2, TAttr3, TAttr4>(this MethodDefinition method)
            where TAttr1 : struct, IMethodAttribute
            where TAttr2 : struct, IMethodAttribute
            where TAttr3 : struct, IMethodAttribute
            where TAttr4 : struct, IMethodAttribute
        {
            return method.SetMethodAttributes<TAttr2, TAttr3, TAttr4>()
                .SetMethodAttributes<TAttr1>();
        }

        public static MethodDefinition SetMethodAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5>(
            this MethodDefinition method)
            where TAttr1 : struct, IMethodAttribute
            where TAttr2 : struct, IMethodAttribute
            where TAttr3 : struct, IMethodAttribute
            where TAttr4 : struct, IMethodAttribute
            where TAttr5 : struct, IMethodAttribute
        {
            return method.SetMethodAttributes<TAttr2, TAttr3, TAttr4, TAttr5>()
                .SetMethodAttributes<TAttr1>();
        }

        public static MethodDefinition SetMethodAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6>(
            this MethodDefinition method)
            where TAttr1 : struct, IMethodAttribute
            where TAttr2 : struct, IMethodAttribute
            where TAttr3 : struct, IMethodAttribute
            where TAttr4 : struct, IMethodAttribute
            where TAttr5 : struct, IMethodAttribute
            where TAttr6 : struct, IMethodAttribute
        {
            return method.SetMethodAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6>()
                .SetMethodAttributes<TAttr1>();
        }

        public static MethodDefinition SetMethodAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7>(
            this MethodDefinition method)
            where TAttr1 : struct, IMethodAttribute
            where TAttr2 : struct, IMethodAttribute
            where TAttr3 : struct, IMethodAttribute
            where TAttr4 : struct, IMethodAttribute
            where TAttr5 : struct, IMethodAttribute
            where TAttr6 : struct, IMethodAttribute
            where TAttr7 : struct, IMethodAttribute
        {
            return method.SetMethodAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7>()
                .SetMethodAttributes<TAttr1>();
        }

        public static MethodDefinition SetMethodAttributes<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7,
            TAttr8>(this MethodDefinition method)
            where TAttr1 : struct, IMethodAttribute
            where TAttr2 : struct, IMethodAttribute
            where TAttr3 : struct, IMethodAttribute
            where TAttr4 : struct, IMethodAttribute
            where TAttr5 : struct, IMethodAttribute
            where TAttr6 : struct, IMethodAttribute
            where TAttr7 : struct, IMethodAttribute
            where TAttr8 : struct, IMethodAttribute
        {
            return method.SetMethodAttributes<TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7, TAttr8>()
                .SetMethodAttributes<TAttr1>();
        }

        public static void MakeForInterface(this   MethodDefinition   method)
        {
            method.Attributes |= MethodAttributes.Abstract;
            method.Attributes |= MethodAttributes.Virtual;
            method.Attributes |= MethodAttributes.HideBySig;
            method.Attributes |= MethodAttributes.NewSlot;
        }
    }
}
