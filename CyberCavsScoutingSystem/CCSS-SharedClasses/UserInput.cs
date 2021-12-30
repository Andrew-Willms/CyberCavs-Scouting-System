using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSS_SharedClasses {

	public class UserInput<T> {


		public delegate T InputValidatorDelegate(string inputString, out List<UserInputValidationError> errors);


		delegate void Del(string str);

		public string InputString { get; set; }

		public T Value { get; private set; }

		public bool IsValid { get; private set; }

		public List<UserInputValidationError> ErrorsList { get; set; }



		public InputValidatorDelegate InputValidator { get; set; }


		public void ValidateInput() {

			if (InputValidator == null) {
				throw new ArgumentNullException("The InputValidator delegate is null");
			}

			Value = InputValidator.Invoke(InputString, out List<UserInputValidationError> errorsList);
			ErrorsList = errorsList;

			IsValid = ErrorsList.Count == 0;
		}


		public static implicit operator UserInput<T>(string inputString) {
			return new() { InputString = inputString };
		}

		public override string ToString() {
			return InputString;
		}

	}

}
