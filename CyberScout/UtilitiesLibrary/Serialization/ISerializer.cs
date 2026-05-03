namespace UtilitiesLibrary.Serialization;



public interface ISerializable<T, out TSelf> where TSelf : ISerializable<T, TSelf> {

	public static abstract TSelf ToSerializable(T value);

	public T FromSerializable();

}