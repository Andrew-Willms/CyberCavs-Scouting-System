using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace UtilitiesLibrary; 



public class SomethingAttribute<T> : Attribute where T : INotifyPropertyChanged {

	public SomethingAttribute(string propertyName) {

	}

}



public class Test : INotifyPropertyChanged {

	[Something<Test>(nameof(Text))]
	public string Text { get; set; }

	public void Function() {

		IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());

		IEnumerable<PropertyInfo> properties = types.SelectMany(x => x.GetProperties());

		//IEnumerable<PropertyInfo> propertyInfos = properties.Where(x => x.GetCustomAttributes(typeof(SomethingAttribute), true).Any());

		//foreach (PropertyInfo propertyInfo in propertyInfos) { }

	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

}