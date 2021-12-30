using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSS_SharedClasses {

	public class UserMultiInput<T> {



		private List<PropertyInfo> InputProperties { get; init; }

		public List<UserInputValidationError> ErrorsList { get; set; }



		public string this[string propertyName] {

			set {
				if (!InputProperties.Select(x => x.Name).Contains(propertyName)) {

					if (typeof(T).GetProperties().Select(x => x.Name).Contains(propertyName)) {

						throw new ArgumentException($"The property \"{propertyName}\" exists but is not designated " +
													$"as an input property for this UserMultiInput object");

					} else {

						throw new ArgumentException($"The property \"{propertyName}\" does not exist in the type of " +
													$"this UserMutliInput object, {typeof(T)}");
					}
				}



			}
		}

	}

}
