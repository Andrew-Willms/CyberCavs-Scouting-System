namespace UtilitiesLibrary.MiscExtensions; 



public static class OptionalExtensions {

	public static Optional<T> Optionalize<T>(this T value) {
		return new(value);
	}

}