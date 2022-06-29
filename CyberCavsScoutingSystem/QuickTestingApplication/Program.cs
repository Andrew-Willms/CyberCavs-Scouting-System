using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;



namespace QuickTestingApplication;

public class Program {

	static void Main(string[] args) {

		//Console.WriteLine("Test");
		//Trace.WriteLine("Test");



	}



	public void TestGenericTypeMatching() {

		ITestInterface<int, int> test = new TestClass1<int, int>();

		Console.WriteLine(test is ITestInterface<int, int>);
		Console.WriteLine(test is TestClass1<int, int>);
		Console.WriteLine(test is TestClass1<int, int, int>);

		Console.WriteLine(test.GetType());
		Console.WriteLine(test.GetType().GetGenericArguments());
		Console.WriteLine(test.GetType().GetGenericTypeDefinition());

		Console.WriteLine(test.GetType().GetGenericTypeDefinition() == typeof(TestClass1<,>));
		Console.WriteLine(test.GetType().GetGenericTypeDefinition() == typeof(TestClass1<,,>));
	}

	public interface ITestInterface<T1, T2> {}

	public class TestClass1<T1, T2> : ITestInterface<T1, T2> {}

	public class TestClass1<T1, T2, T3> : ITestInterface<T1, T2> {}



	private static void TestReadonlyCollection() {

		List<string> testList = new() { "test1", "test2" };

		PrintList(testList);

		IReadOnlyList<string> testAsStringOnly = testList.AsReadOnly();

		//List<string> castOfReadonly = (List<string>)testAsStringOnly; // This cast causes an exception
		List<string> castOfReadonly = testAsStringOnly.ToList(); // This creates a copy

		castOfReadonly.Add("test3");

		PrintList(castOfReadonly);
		PrintList(testList);
	}

	private static void PrintList(List<string> list) {

		Console.Write("{ ");

		for (int i = 0; i < list.Count - 1; i++) {
			Console.Write(list[i] + ", ");
		}

		Console.WriteLine(list[^1] + " }");
	}

	public static (int testInt, string testString, double testDouble) test = (1, "1", 1);
}