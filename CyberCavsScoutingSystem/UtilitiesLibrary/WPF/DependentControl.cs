using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace UtilitiesLibrary.WPF;



[AttributeUsage(AttributeTargets.Property)]
public class DependentAttribute : Attribute {

	public string SingletonPropertyName { get; }

	public DependentAttribute(string singletonPropertyName) {

		SingletonPropertyName = singletonPropertyName;
	}

}



public abstract class DependentControl<TSingleton> : UserControl, INotifyPropertyChanged
	where TSingleton : INotifyPropertyChanged {

	protected abstract TSingleton SingletonGetter { get; }

	protected DependentControl() {

		IEnumerable<PropertyInfo> singletonProperties = typeof(TSingleton).GetProperties();

		IEnumerable<PropertyInfo> dependentProperties = GetType().GetProperties()
			.Where(x => x.GetCustomAttributes(typeof(DependentAttribute), true).Any());

		foreach (PropertyInfo dependentProperty in dependentProperties) {

			IEnumerable<DependentAttribute> dependentAttributes = dependentProperty.GetCustomAttributes<DependentAttribute>();

			foreach (DependentAttribute dependentAttribute in dependentAttributes) {

				string singletonPropertyName = dependentAttribute.SingletonPropertyName;
				PropertyInfo? singletonProperty = singletonProperties.FirstOrDefault(x => x.Name == singletonPropertyName);

				if (singletonProperty is null) {
					throw new InvalidOperationException($"The type {typeof(TSingleton)} does not have a property named {singletonPropertyName}.");
				}

				// This call to a abstract member is fine since the member is auto initialized (before constructor).
				// ReSharper disable once VirtualMemberCallInConstructor 
				SingletonGetter.PropertyChanged += (_, args) => PropertyChangedEventHandler(args, singletonPropertyName, dependentProperty.Name);
			}
		}
	}

	private void PropertyChangedEventHandler(PropertyChangedEventArgs args, string singletonProperty, string dependentPropertyName) {

		if (args.PropertyName == singletonProperty) {
			OnPropertyChanged(dependentPropertyName);
		}
	}

	protected abstract void OnPropertyChanged(string propertyName);

	public abstract event PropertyChangedEventHandler? PropertyChanged;

}