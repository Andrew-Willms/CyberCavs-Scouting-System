using System;
using System.Linq;
using System.Reflection;

namespace UtilitiesLibrary.MiscExtensions;



public static class TypeExtensions {

	public static bool Inherits(this Type type, Type potentialParent) {

		if (!type.IsClass) {
			throw new InvalidOperationException();
		}

		for (Type? current = type; current is not null; current = current.BaseType) {

			if (current.IsDirectlyAssignableTo(potentialParent)) {
				return true;
			}
		}

		return false;
	}

	public static bool Implements(this Type type, Type @interface) {

		if (!@interface.IsInterface) {
			throw new InvalidOperationException();
		}

		return type.IsDirectlyAssignableTo(@interface) ||
			   type.GetInterfaces().Any(interfaceType => interfaceType.IsDirectlyAssignableTo(@interface));
	}



	public static bool IsIndirectlyAssignableTo(this Type type, Type targetType) {

		if (targetType.IsInterface) {
			return type.Implements(targetType);
		}

		if (targetType.IsClass) {
			type.Inherits(targetType);
		}

		if (targetType.IsValueType) {
			return type.IsAssignableTo(targetType);
		}

		throw new NotSupportedException();
	}

	public static bool IsDirectlyAssignableTo(this Type type, Type targetType) {

		if (type == targetType) {
			return true;
		}

		if (!type.IsGenericType || !targetType.IsGenericType) {
			return false;
		}

		if (targetType.IsOpenGeneric()) {
			return type.GetGenericTypeDefinition() == targetType.GetGenericTypeDefinition();
		}

		if (type.GetGenericTypeDefinition() != targetType.GetGenericTypeDefinition()) {
			return false;
		}

		return type.GetGenericArguments()
			.Zip(targetType.GetGenericArguments())
			.All(x => x.First.IsDirectlyAssignableTo(x.Second));
	}



	public static bool IsValidGenericTypeParameter(this Type type, Type typeParameter) {

		if (!typeParameter.IsGenericTypeParameter) {
			throw new InvalidOperationException();
		}

		bool mustBeReferenceType = (typeParameter.GenericParameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint) != 0;
		bool mustBeNullableValueType = (typeParameter.GenericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0;
		bool mustHaveDefaultConstructor = (typeParameter.GenericParameterAttributes & GenericParameterAttributes.DefaultConstructorConstraint) != 0;

		if (mustBeReferenceType && type.IsValueType) {
			return false;
		}

		if (mustBeNullableValueType && (type.IsReferenceType() || !type.IsNullableType())) {
			return false;
		}

		if (mustHaveDefaultConstructor && !type.HasDefaultConstructor()) {
			return false;
		}

		return typeParameter.GetGenericParameterConstraints().All(type.IsIndirectlyAssignableTo);
	}



	public static bool HasDefaultConstructor(this Type type) {

		return type.IsValueType || type.GetConstructor(Type.EmptyTypes) != null;
	}

	public static bool IsReferenceType(this Type type) {

		return !type.IsValueType;
	}

	public static bool IsClosedNullableType(this Type type) {

		return Nullable.GetUnderlyingType(type) != null;
	}

	public static bool IsNullableType(this Type type) {

		return type.IsClosedNullableType() || type.Name == typeof(Nullable<>).Name;
	}



	public static bool IsOpenGeneric(this Type type) {

		return type is { IsGenericType: true, IsGenericTypeDefinition: true };
	}

	public static bool IsClosedGeneric(this Type type) {

		return type is { IsGenericType: true, IsGenericTypeDefinition: false };
	}



	public static Type[] GetNestedClasses(this Type type) {

		return type.GetNestedTypes().Where(x => x.IsClass).ToArray();
	}

	public static Type[] GetNestedInterfaces(this Type type) {

		return type.GetNestedTypes().Where(x => x.IsInterface).ToArray();
	}

	public static Type[] GetClasses(this Assembly assembly) {

		return assembly.GetTypes().Where(x => x.IsClass).ToArray();
	}

	public static Type[] GetInterfaces(this Assembly assembly) {

		return assembly.GetTypes().Where(x => x.IsInterface).ToArray();
	}

}