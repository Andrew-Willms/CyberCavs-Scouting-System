using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace QuickTestingApplication;



public class Program {

	private static void Main(string[] args) {

		//TestGenericTypeMatching();

		Console.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));

		//Console.WriteLine(typeof(int?));
		//Console.WriteLine(typeof(int?).Name);
		//Console.WriteLine(typeof(Nullable<>));
		//Console.WriteLine(typeof(Nullable<>).Name);

		//Type test1 = typeof(B<>).BaseType.GetGenericArguments()[0];
		//Type test2 = typeof(B<int>).BaseType.GetGenericArguments()[0];

		//Console.WriteLine(test1);
		//Console.WriteLine(test2);
	}


	public static void TestGenericTypeMatching() {

		ITestInterface<int, int> test = new TestClass1<int, int>();

		Console.WriteLine(typeof(TestClass1<,>).IsGenericType); // true
		Console.WriteLine(typeof(TestClass1<int, int>).IsGenericType); // true
		Console.WriteLine(typeof(TestClass1<,>).IsGenericTypeDefinition); // true
		Console.WriteLine(typeof(TestClass1<int,int>).IsGenericTypeDefinition); // false
		Console.WriteLine(typeof(TestClass1<,>).IsTypeDefinition); // false
		Console.WriteLine(typeof(TestClass1<int, int>).IsTypeDefinition); // false

		Console.WriteLine(typeof(TestClass1<,>).GetGenericTypeDefinition().IsGenericType); // true
		Console.WriteLine(typeof(TestClass1<int, int>).GetGenericTypeDefinition().IsGenericType); // true

		PrintEnumerable(typeof(TestClass1<int,int>).GetGenericArguments());
		PrintEnumerable(typeof(TestClass1<int,int>).GetGenericTypeDefinition().GetGenericArguments());

		PrintEnumerable(typeof(TestClass1<,>).GetGenericArguments());
		PrintEnumerable(typeof(TestClass1<,>).GetGenericTypeDefinition().GetGenericArguments());

		Console.WriteLine(test is ITestInterface<int, int>); // true
		Console.WriteLine(test is TestClass1<int, int>); // true

		Console.WriteLine(typeof(TestClass1<int, int>)); 
		Console.WriteLine(typeof(TestClass1<,>));

		Console.WriteLine(test.GetType());
		Console.WriteLine(test.GetType().GetGenericTypeDefinition());
		Console.WriteLine(test.GetType().GetGenericTypeDefinition().GetGenericTypeDefinition());

		Console.WriteLine(test.GetType() == typeof(TestClass1<,>)); // false
		Console.WriteLine(test.GetType().IsAssignableTo(typeof(TestClass1<,>))); // false

		Console.WriteLine(test.GetType().GetGenericTypeDefinition() == typeof(TestClass1<,>)); // true
		Console.WriteLine(test.GetType().GetGenericTypeDefinition().IsAssignableTo(typeof(TestClass1<,>))); // true

		Console.WriteLine(test.GetType() == typeof(TestClass1<int, int>)); // true
		Console.WriteLine(test.GetType().IsAssignableTo(typeof(TestClass1<int, int>))); // true
	}



	private static void PrintEnumerable<T>(IEnumerable<T> enumerable) {

		List<T> list = enumerable.ToList();

		Console.Write("{ ");

		for (int i = 0; i < list.Count - 1; i++) {
			Console.Write(list[i] + ", ");
		}

		Console.WriteLine(list[^1] + " }");
	}

}



public class A<T> { }

public class B<TTest> : A<TTest> { }

public interface IC { }


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