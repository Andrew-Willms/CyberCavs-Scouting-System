using System;

namespace UtilitiesLibrary.MiscExtensions; 



public static class TypeExtensions {

	public static bool IsSubclassOfGeneric(this Type toCheck, Type generic) {

		if (!toCheck.IsClass) {
			return false;
		}

		while (true) {

			Type current = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;

			if (generic == current) {
				return true;
			}

			if (toCheck == typeof(object)) {
				break;
			}

			toCheck = toCheck.BaseType!;
		}

		return false;
	}

}