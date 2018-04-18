using System;

// ReSharper disable CheckNamespace
namespace Mono.Cecil.Fluent
{
    public sealed class SystemTypeOrTypeReference
    {
        private readonly Type          _type;
        private readonly TypeReference _typeRef;

        internal SystemTypeOrTypeReference(Type t)
        {
            if (t == null) throw new ArgumentNullException();
            _type = t;
        }

        internal SystemTypeOrTypeReference(TypeReference t)
        {
            if (t == null) throw new ArgumentNullException();
            _typeRef = t;
        }

        internal TypeReference GetTypeReference(ModuleDefinition module)
        {
            if (_type != null) return module.SafeImport(_type);
            if (_typeRef != null) return module.SafeImport(_typeRef);
            throw new ArgumentException();
        }

        public static implicit operator SystemTypeOrTypeReference(Type t)
        {
            return new SystemTypeOrTypeReference(t);
        }

        public static implicit operator SystemTypeOrTypeReference(TypeReference t)
        {
            return new SystemTypeOrTypeReference(t);
        }
    }
}