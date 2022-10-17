// ReSharper disable ConvertToCompoundAssignment
// ReSharper disable RedundantAssignment
// ReSharper disable UnusedVariable
// ReSharper disable NotAccessedVariable
// ReSharper disable ConvertIfStatementToSwitchStatement

// ReSharper disable ConvertToConstant.Local
#pragma warning disable CS8600
#pragma warning disable CS0219



// A variable in c# is like a variable in math, it is a name like "x", "v_max", that has a value that can change.

// In math all variables are numbers but in c# a variables can have different types.

// This is how we "declare/define" (tell the computer that our variable exists):
int anInteger = 6;

// When declaring a variable the first thing we wright is the variable type, in this case the type is an int.
// An int can store an integer from -2,147,483,648 to 2,147,483,647.

// Second is the variable name. This is like "x" or "x_max" in math.

// Third is an equals sign, this assigns the value on the right (in this case 6) to the variable.

// The last thing on the line is a semi colon, this tells the computer that this is the end of one of our statement.
// A statement is like a single instruction for the computer.


// Once we have a variable we can operate on it.
// Notice how we don't start the line with "int". We specify the type where we declare the variable.
anInteger = 10;				// Sets the value of anInteger to 10, the old value (6) is forgotten.

anInteger = 1 + 1;			// We can do math, sets the value to 2
anInteger = 1 + 2 * 4 - 1;	// sets the value to 8
anInteger = 10 / 2;			// sets the value to 5
anInteger = 11 / 2;			// sets the value to 5, when you are dividing one int by another int the result is always rounded down

anInteger = anInteger + 2;	// anInteger is currently 5, 5 + 2 is 7, so this sets the value to 7
anInteger += 2;				// this adds 2 to the value of anInteger so it's now 9
anInteger -= 2;				// this subtracts 2 so it's back to 7
anInteger += anInteger;		// this adds the value of the integer to itself, doubling it to 14
anInteger++;				// this adds 1 to the value so it's now 15
anInteger--;				// and this subtracts 1 so it's now 14



// a variable of double type number can store a decimal number or an integer from -1.7978x10^308 to 1.7978x10^308
double aDecimalNumber = 3.14159;	// sets the value to the first 6 digits of pie
aDecimalNumber = 1e100;				// sets the value to 1x10^100


// a bool is a variable type that can only be true or false
bool isItDayTime = false;
isItDayTime = true;
isItDayTime = !true; // ! means "not" so this says "set the variable to not true" (i.e. false)


// strings are used to save text
string yourName = "Andrew";
string greeting = "Hello " + yourName; // this now stores "Hello Andrew"




// This is a function. A function is much like a function in math.
// It can take 0 or more variables as "parameters" aka "arguments" and "return" a new value.
// In math you might say that f(x) evaluates to a value, in programming we say SomeFunction() "returns" a value
// Like with variables in programming we have to define the type the function will return and the type of any arguments.

//<returnType> <functionName>(<any parameters/arguments here>) { }
// anything inside the {} is called the "function body".
// parameters that are defined within the () can be used as variables within the function.
// the return keyword tells the compiler what value the function should return.
// In this case the function returns the parameter "numberToDouble" multiplied by itself

// This function returns an int type, is named "Square", and takes an int value as a parameter.
int Square(int numberToSquare) {
	return numberToSquare * numberToSquare;
}

// Can use a function we define. 
int anotherInteger = Square(4); // this creates a new int variable called anotherInteger and sets the value to 16.
anotherInteger = Square(anInteger); // this sets the value of to 16 square, 256









// "Console" is a static class defined in the "System" namespace.
// We can talk about "static" and "class" mean later.

// WriteLine() is a function defined in the Console class.
// We access functions of a class by using ClassName.FunctionName(<any parameters/arguments here>)

// Console.WriteLine() is a function can take anything as an argument and print that thing out to the console.
// Console.ReadLine() is a function that waits for the user to type text and press enter then saves that text to a variable

Console.WriteLine("Enter your name: ");
string name = Console.ReadLine();


// If statements control the flow of code. If the value, known as the condition, inside the () of
// the if statement is true then the code inside the {} is executed, otherwise it is skipped
// The () of the if statement must contain something that can be evaluated to a bool (either true or false).

// The == operator is called the comparison operator.
// It evaluates the value to the left and right and returns true if they are the same and false if they are different.
if (name == "Andrew Veldhuise") {
	Console.WriteLine("You're not the real Andrew.");

// If statements can be followed by else if statements. If the condition of the if statement is false and the 
// condition in an if else statement is true then the code inside the {} of the else if statement is executed.
} else if (name == "Andrew Willms") {
	Console.WriteLine("Welcome supreme chancellor.");

// You can chain as many else if statements as you want.
} else if (name == "Dwayne the Rock Johnson") {
	Console.WriteLine("Wow, didn't expect to see you here.");

} else {
	Console.WriteLine("Hello" + name + ".");
}



/*
 * Ideas for programming activities:
 * - Write a function that finds the largest number in a list of integers.
 * - Write a function that determines if an integer parameter is prime.
 * - Write a function that multiplies two integers together without using the multiplication sign
 * - Make a person class with a Name, a Player child class with a goals scored property,
 *   and a team class with a Coach and Matches won property.
 *
 *
 *
 */