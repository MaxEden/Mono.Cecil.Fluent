using System;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
    public static partial class TypeDefinitionExtensions
    {
        public static bool Implements(this TypeDefinition type, string interfaceFullName)
        {
            try
            {
                if (type.HasInterfaces && type.Interfaces.Any(p => p.FullName == interfaceFullName)) return true;
                if (type.BaseType == null) return false;
                return Implements(type.BaseType.Resolve(), interfaceFullName);
            }
            catch
            {
                return false;
            }
        }

        public static bool DerivedFrom(this TypeDefinition type, string typeFullName)
        {
            if (type.BaseType == null) return false;
            return DerivedFrom(type.BaseType.Resolve(), typeFullName);
        }

        public static bool IsSubclassOf(this TypeDefinition type, string typeFullName)
        {
            return Implements(type, typeFullName) || DerivedFrom(type, typeFullName);
        }

        public static bool IsEnum(this TypeReference type)
        {
            return type.IsValueType && !type.IsPrimitive && type.Resolve().IsEnum;
        }

        public static bool IsStruct(this TypeReference type)
        {
            return type.IsValueType && !type.IsPrimitive && !type.IsEnum() && !IsSystemDecimal(type);
        }

        private static bool IsSystemDecimal(TypeReference type)
        {
            return type.FullName == "System.Decimal";
        }
    }
}