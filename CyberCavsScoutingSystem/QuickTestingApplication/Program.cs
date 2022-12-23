using System;
using System.Collections.Generic;


namespace QuickTestingApplication;

public class Program {

	private static void Main(string[] args) {

		//Console.WriteLine("Test");
		//Trace.WriteLine("Test");

		//Child test = new Child();

		TestGenericTypeMatching();
	}



	public static void TestGenericTypeMatching() {

		ITestInterface<int, int> test = new TestClass1<int, int>();

		Console.WriteLine(test is ITestInterface<int, int>);
		Console.WriteLine(test is TestClass1<int, int>);
		Console.WriteLine(test is TestClass1<int, int, int>);

		Console.WriteLine(test.GetType());
		Console.WriteLine(test.GetType().GetGenericArguments());
		Console.WriteLine(test.GetType().GetGenericTypeDefinition());

		Console.WriteLine(test.GetType() == typeof(TestClass1<,>));
		Console.WriteLine(test.GetType() == typeof(TestClass1<int, int>));
		Console.WriteLine(test.GetType().GetGenericTypeDefinition() == typeof(TestClass1<,>));
		Console.WriteLine(test.GetType() == typeof(TestClass1<,,>));
		Console.WriteLine(test.GetType().GetGenericTypeDefinition() == typeof(TestClass1<,,>));
	}



	private static void PrintList(List<string> list) {

		Console.Write("{ ");

		for (int i = 0; i < list.Count - 1; i++) {
			Console.Write(list[i] + ", ");
		}

		Console.WriteLine(list[^1] + " }");
	}

}



public class Parent {

	public Parent() {

		Console.WriteLine(GetType() == typeof(Parent));
		Console.WriteLine(GetType() == typeof(Child));
	}

}

public class Child : Parent {

}


public interface ITestInterface<T1, T2> {}

public class TestClass1<T1, T2> : ITestInterface<T1, T2> {}

public class TestClass1<T1, T2, T3> : ITestInterface<T1, T2> {}



public class TestIsNullInConstructor {

	public TestIsNullInConstructor() {

		Console.WriteLine("from constructor " + (this is null));

		SomeOtherFunction(this);
	}

	private static void SomeOtherFunction(TestIsNullInConstructor? test) {
		Console.WriteLine("from other function " + (test is null));
	}

}