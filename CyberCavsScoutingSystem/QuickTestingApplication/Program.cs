﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickTestingApplication;

public class Program {

	static void Main(string[] args) {

		

	}

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