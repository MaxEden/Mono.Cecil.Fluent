using System;
using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
	partial class ModuleDefinitionExtensions
	{
		private static readonly object SyncRoot = new object(); // todo: really needed?

		public static TypeReference SafeImport(this ModuleDefinition module, Type type)
		{
			lock (SyncRoot)
				return module.ImportReference(type);
		}

		public static TypeReference SafeImport<T>(this ModuleDefinition module)
		{
			lock (SyncRoot)
				return module.ImportReference(typeof(T));
		}

        public static TypeReference SafeImportOpen<T>(this ModuleDefinition module)
        {
            lock (SyncRoot)
            {
                var type = SafeImport<T>(module) as GenericInstanceType;
                if (type == null) throw new ArgumentException();

                return type.ElementType;
            }
        }

        public static MethodReference SafeImportOpen<T>(this ModuleDefinition module, Expression<Action<T>> expression)
	    {
	        lock (SyncRoot)
	        {
	            var method = SafeImport(module, expression) as GenericInstanceMethod;
	            if (method == null) throw new ArgumentException();

	            return method.ElementMethod;
	        }
	    }

        public static MethodReference SafeImportOpen<T>(this ModuleDefinition module, Expression<Func<T, object>> expression)
        {
            lock (SyncRoot)
            {
                var method = SafeImport(module, expression) as GenericInstanceMethod;
                if (method == null) throw new ArgumentException();

                return method.ElementMethod;
            }
        }

        public static MethodReference SafeImport<T>(this ModuleDefinition module, Expression<Action<T>> expression)
        {
            var body = expression.Body;
            if (body is UnaryExpression uEx) body = uEx.Operand;

            if (body is MethodCallExpression methodCall)
            {
                lock (SyncRoot)
                    return module.ImportReference(methodCall.Method);
            }

            if (body is MemberExpression member && member.Member is PropertyInfo prop)
            {
                lock (SyncRoot)
                    return module.ImportReference(prop.SetMethod);
            }

            throw new ArgumentException(nameof(expression));
        }

        public static MethodReference SafeImport<T>(this ModuleDefinition module, Expression<Func<T, object>> expression)
        {
            var body = expression.Body;
            if (body is UnaryExpression uEx) body = uEx.Operand;

            if (body is MethodCallExpression methodCall)
            {
                lock (SyncRoot)
                    return module.ImportReference(methodCall.Method);
            }

            if (body is MemberExpression member && member.Member is PropertyInfo prop)
            {
                lock (SyncRoot)
                    return module.ImportReference(prop.GetMethod);
            }

            throw new ArgumentException(nameof(expression));
        }

        public static TypeReference SafeImport(this ModuleDefinition module, TypeReference type)
		{
			lock (SyncRoot)
				return module.ImportReference(type);
		}

		public static FieldReference SafeImport(this ModuleDefinition module, FieldReference field)
		{
			lock (SyncRoot)
				return module.ImportReference(field);
		}

		public static MethodReference SafeImport(this ModuleDefinition module, MethodReference method)
		{
			lock (SyncRoot)
				return module.ImportReference(method);
		}

		public static FieldReference SafeImport(this ModuleDefinition module, FieldInfo field)
		{
			lock (SyncRoot)
				return module.ImportReference(field);
		}

		public static MethodReference SafeImport(this ModuleDefinition module, MethodInfo method)
		{
			lock (SyncRoot)
				return module.ImportReference(method);
		}
        
		public static MethodReference SafeImport(this ModuleDefinition module, ConstructorInfo constructor)
		{
			lock (SyncRoot)
				return module.ImportReference(constructor);
		}
	}
}
