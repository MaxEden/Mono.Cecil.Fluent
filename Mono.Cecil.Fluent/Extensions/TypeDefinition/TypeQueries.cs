using System.Linq;
using Mono.Cecil.Rocks;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
    public static partial class TypeDefinitionExtensions
    {

        public static bool Implements<T>(this TypeReference type) where T : class
        {
            return Implements(type, typeof(T).FullName);
        }

        public static bool Implements(this TypeReference typeRef, string interfaceFullName)
        {
            var type = typeRef as TypeDefinition;
            if(type == null) type = typeRef.Resolve();

            if(type == null && typeRef is GenericParameter genericParameter)
            {
                foreach(var constraint in genericParameter.Constraints)
                {
                    if(constraint.Implements(interfaceFullName)) return true;
                }

                return false;
            }

            try
            {
                if (type.HasInterfaces && type.Interfaces.Any(p => p.InterfaceType.FullName == interfaceFullName)) return true;
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
            if (type.BaseType.FullName == typeFullName) return true;
            return DerivedFrom(type.BaseType.Resolve(), typeFullName);
        }

        public static bool IsSubclassOf(this TypeDefinition type, TypeReference baseType)
        {
            return Implements(type, baseType.FullName) || DerivedFrom(type, baseType.FullName);
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