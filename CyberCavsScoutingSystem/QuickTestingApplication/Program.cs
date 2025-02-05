using System;
using UtilitiesLibrary.Optional;

namespace QuickTestingApplication;



public class Program {

	private static void Main(string[] args) {

		for (int i = 0; i < 3; i++) {

			switch (i) {
				case 0:
					Console.WriteLine(0);
					continue;
				case 1:
					Console.WriteLine(1);
					continue;
				case 2:
					Console.WriteLine(2);
					continue;
			}

		}
	}
}