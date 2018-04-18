﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

// ReSharper disable once CheckNamespace
namespace Mono.Cecil.Fluent
{
    partial class FluentEmitter
    {
        private static bool AreParameterListsEqual(IEnumerable<TypeReference> plist1, IEnumerable<TypeReference> plist2)
        {
            var plist1Array = plist1 as TypeReference[] ?? plist1.ToArray();
            var plist2Array = plist2 as TypeReference[] ?? plist2.ToArray();

            if (plist1Array.Length != plist2Array.Length)
                return false;

            var i = 0;
            foreach (var p in plist1Array)
            {
                if (!p.SafeEquals(plist2Array.ElementAt(i)))
                    return false;
                ++i;
            }

            return true;
        }

        public FluentEmitter NewObj<T>(params SystemTypeOrTypeReference[] paramtypes)
        {
            return NewObj(typeof(T), paramtypes);
        }

        public FluentEmitter NewObj(SystemTypeOrTypeReference type, params SystemTypeOrTypeReference[] paramtypes)
        {
            // todo: generic and base constructors don't work currently
            // todo: newobj for primitives should emit initobj and not throw any exception

            var typeRef = type.GetTypeReference(Module);
            var typeDef = typeRef.Resolve();

            if (typeRef.IsPrimitive)
                throw new Exception("primitive value types like int, bool, long .. don't have a constructor. newobj instruction not possible");

            var constructors = typeRef.Resolve().GetConstructors()
                .Where(c => AreParameterListsEqual(c.Parameters.Select(p => p.ParameterType), paramtypes.Select(p => p.GetTypeReference(Module)))).ToList();

            if(constructors.Count() != 1)
                throw new Exception("Can not find constructor"); // todo: better exception info, ncrunch: no coverage

            MethodReference ctor = constructors.First();
            if (typeRef is GenericInstanceType genRef)
            {
                ctor = ctor.MakeGeneric(genRef.GenericArguments.ToArray());
            }

            return Emit(OpCodes.Newobj, ctor);
        }

        public FluentEmitter NewObj(ConstructorInfo constructor)
        {
            return NewObj(constructor.DeclaringType, constructor.GetParameters().Select(p => new SystemTypeOrTypeReference(p.ParameterType)).ToArray());
        }
    }
}
