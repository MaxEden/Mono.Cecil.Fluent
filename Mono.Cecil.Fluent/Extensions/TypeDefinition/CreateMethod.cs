using Mono.Cecil.Fluent.Utils;
using Mono.Cecil.Rocks;

// ReSharper disable InconsistentNaming

namespace Mono.Cecil.Fluent
{
	public static partial class IMemberDefinitionExtensions
	{
		public static MethodDefinition CreateMethod(this TypeDefinition type, string name = null, SystemTypeOrTypeReference returnType = null, MethodAttributes? attributes = null)
		{
			var module = type.GetModule();
			var t = returnType != null ? returnType.GetTypeReference(type.GetModule()) : module.TypeSystem.Void;
			var method = new MethodDefinition(name ?? Generate.Name.ForMethod(), attributes ?? 0, t);

		    if(type is TypeDefinition definition) 
				definition.Resolve().Methods.Add(method);
			else
				type.DeclaringType.Methods.Add(method);

		    return method;
		}

		public static MethodDefinition CreateMethod<T>(this TypeDefinition type, string name = null, MethodAttributes? attributes = null)
		{
			return CreateMethod(type, name, typeof(T), attributes);
		}

		public static MethodDefinition CreateMethod(this TypeDefinition type, SystemTypeOrTypeReference returnType, MethodAttributes? attributes = null)
		{
			return CreateMethod(type, null, returnType.GetTypeReference(type.GetModule()), attributes);
		}

        public static MethodDefinition GetOrCreateStaticConstructor(this TypeDefinition type)
        {
            var staticCtor = type.GetStaticConstructor();
            if (staticCtor == null)
            {
                staticCtor = new MethodDefinition(
                    ".cctor",
                    MethodAttributes.Private
                    | MethodAttributes.HideBySig
                    | MethodAttributes.SpecialName
                    | MethodAttributes.RTSpecialName
                    | MethodAttributes.Static,
                    type.Module.TypeSystem.Void);
                type.Methods.Add(staticCtor);
            }

            return staticCtor;
        }

    }
}
