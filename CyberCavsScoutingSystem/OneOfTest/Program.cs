using System.Diagnostics;
using OneOf;

namespace OneOfTest;



internal class Program {

	private static void Main(string[] args) {
			



	}

	private static void ConsumingFunction() {

		ThingResult result = ThingFunction(true);

		object @object = null!;

		switch (@object) {
			case int number:
				Console.WriteLine(number);
				break;
			default:
				throw new UnreachableException();
		}

		switch (result) {

			case int number:
				Console.WriteLine(number);
				break;
			case ThingError error:
				Console.WriteLine(error);
				break;
			default:
				throw new UnreachableException();
		}

	}

	private static ThingResult ThingFunction(bool succeed) {

		return succeed
			? 42
			: new ThingError();
	}

}


public interface ISuccess;

public interface IValueSuccess<T> : ISuccess;

public interface IError;

public class ThingError : {

}

public class ThingSuccess {

}

public class ThingSuccessWithValue {

}

//[GenerateOneOf]
public partial class ThingResult : OneOfBase<int, ThingError> {

	protected ThingResult(OneOf<int, ThingError> input) : base(input) { }

	public static implicit operator ThingResult(int integer) => new(integer);

	public static implicit operator ThingResult(ThingError error) => new(error);

	public static implicit operator int(ThingResult result) {
		return 4;
	}

	public static implicit operator ThingError(ThingResult result) {

		return result.Match(
			number => throw new UnreachableException(),
			error => error);

	}

}