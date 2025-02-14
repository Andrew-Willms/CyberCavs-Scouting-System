using System;
using System.Collections.Generic;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Collections;

namespace QuickTestingApplication;



public class Program {

	private static void Main(string[] args) {

		SelectionDataFieldSpec test1 = new() {
			Name = "test",
			Options = new List<string> { "1", "2" }.ToReadOnly(),
			RequiresValue = false
		};

		SelectionDataFieldSpec test2 = new() {
			Name = "test",
			Options = new List<string> { "1", "2" }.ToReadOnly(),
			RequiresValue = false
		};

		BooleanDataFieldSpec test3 = new() {
			Name = "test",
			InitialValue = false,
		};

		BooleanDataFieldSpec test4 = new() {
			Name = "test",
			InitialValue = false,
		};

		Console.WriteLine(test1.Equals(test2));
		Console.WriteLine(test3.Equals(test4));
	}
}