using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSS_SharedClasses {


	// I think these things need to be static
	public record InputStringValidationError {

		public string Name { get; init; }

		public object Tooltip { get; init; }

		//public sometingHere ToolTipStyle { get; init; }
	}



	public class UserInputBindingData<T> {

		public string InputString { get; init; }

		public T Value { get; private set; }

		public bool IsValid { get; private set; }

		public List<InputStringValidationError> ErrorsList { get; set; }



		public Func<string, List<InputStringValidationError>> InputValidator { get; set; }


		public void ValidateInput() {

			if (InputValidator == null) {
				throw new NullReferenceException("The InputValidator Func is null");
			}

			ErrorsList = InputValidator.Invoke(InputString);

			IsValid = ErrorsList.Count == 0;
		}


		public static implicit operator UserInputBindingData<T>(string inputString) {
			return new() { InputString = inputString };
		}

		public override string ToString() {
			return InputString;
		}
	}
}
