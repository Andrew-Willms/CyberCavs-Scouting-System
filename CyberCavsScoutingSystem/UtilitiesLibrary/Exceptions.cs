using System;

namespace UtilitiesLibrary; 



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