using System;
using CCSSDomain.GameSpecification;

namespace QuickTestingApplication;



public class Program {

	private static void Main(string[] args) {

		AllianceColor test = new() {
			Name = "",
			Color = default
		};

		AllianceColor test2 = new() {
			Name = "",
			Color = default
		};

		Console.WriteLine(new B { Test = 1 }.GetHashCode());
		Console.WriteLine(new A { Test = 1 }.GetHashCode());
		Console.WriteLine(1.0.GetHashCode());
		Console.WriteLine(2.0.GetHashCode());
		Console.WriteLine(test);
		Console.WriteLine(test2);
	}
}

public class A {

	public int Test { get; init; }
}

public class B {

	public int Test { get; init; }
}