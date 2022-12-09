using System;

namespace UtilitiesLibrary; 



//TODO see if there are built in exceptions that fill these rolls, I think Nick Chapsas had a video on a relevant exception recently.
public class ShouldNotReachException : Exception {

	public ShouldNotReachException() { }

	public ShouldNotReachException(string message) : base(message) { }

	public ShouldNotReachException(string message, Exception inner) : base(message, inner) { }
}

public class ShouldMatchOtherCaseException : Exception {

	public ShouldMatchOtherCaseException() { }

	public ShouldMatchOtherCaseException(string message) : base(message) { }

	public ShouldMatchOtherCaseException(string message, Exception inner) : base(message, inner) { }
}