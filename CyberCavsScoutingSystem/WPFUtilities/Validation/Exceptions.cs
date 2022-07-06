using System;

namespace WPFUtilities.Validation; 



public class TestException : Exception {

	public TestException() {

	}

	public TestException(string message) : base(message) {

	}

	public TestException(string message, Exception inner) : base(message, inner) {

	}

}


public class NullInputObjectInConverter : Exception {

	public NullInputObjectInConverter() { }

	public NullInputObjectInConverter(string message) : base(message) { }

	public NullInputObjectInConverter(string message, Exception inner) : base(message, inner) { }
}

public class NullInputObjectInInverter : Exception {

	public NullInputObjectInInverter() { }

	public NullInputObjectInInverter(string message) : base(message) { }

	public NullInputObjectInInverter(string message, Exception inner) : base(message, inner) { }
}