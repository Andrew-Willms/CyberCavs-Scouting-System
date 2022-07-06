﻿using System;

namespace WPFUtilities.Validation; 



public class TestException : Exception {

	public TestException() {

	}

	public TestException(string message) : base(message) {

	}

	public TestException(string message, Exception inner) : base(message, inner) {

	}
}