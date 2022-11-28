using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UtilitiesLibrary.Validation;

namespace UtilitiesLibrary;



public static class Extension {

	public static IEnumerable<T> GetCustomAttributes<T>(PropertyInfo propertyInfo, bool inherit = true) where T : Attribute {

		return propertyInfo
			.GetCustomAttributes(typeof(T), inherit)
			.Select(x => x as T ?? throw new ShouldNotReachException("The array should only contain attributes of the correct type"));
	}

}


[AttributeUsage(AttributeTargets.Property)]
public class DependentAttribute<TDependent, TSingleton> : Attribute
	where TDependent : DependentBase<TSingleton>, INotifyPropertyChanged
	where TSingleton : INotifyPropertyChanged {

	public string PropertyName { get; }

	public DependentAttribute(string propertyName) {

		PropertyName = propertyName;
	}

}



public abstract class DependentBase<TSingleton> : INotifyPropertyChanged
	where TSingleton : INotifyPropertyChanged {

	protected abstract TSingleton SingletonGetter { get; }

	protected DependentBase() {

		IEnumerable<PropertyInfo> singletonProperties = typeof(TSingleton).GetProperties();

		Type derivedType = null!; // TODO: Get most derived version of object

		IEnumerable<PropertyInfo> dependentProperties = derivedType.GetProperties()
			.Where(x => x.GetCustomAttributes(typeof(DependentAttribute<DependentBase<TSingleton>, TSingleton>), true).Any());

		foreach (PropertyInfo dependentProperty in dependentProperties) {

			//IEnumerable<DependentAttribute<DependentBase<TSingleton>, TSingleton>> dependentAttributes =
			//	dependentProperty.GetCustomAttributes<DependentAttribute<DependentBase<TSingleton>, TSingleton>>();

			object[] dependentAttributesObjects = dependentProperty.GetCustomAttributes(typeof(DependentAttribute<DependentBase<TSingleton>, TSingleton>), true);

			IEnumerable<DependentAttribute<DependentBase<TSingleton>, TSingleton>> dependentAttributes = dependentAttributesObjects
				.Select(x => x as DependentAttribute<DependentBase<TSingleton>, TSingleton> ?? throw new InvalidOperationException());

			foreach (DependentAttribute<DependentBase<TSingleton>, TSingleton> dependentAttribute in dependentAttributes) {

				string singletonPropertyName = dependentAttribute.PropertyName;
				PropertyInfo? singletonProperty = singletonProperties.FirstOrDefault(x => x.Name == singletonPropertyName);

				if (singletonProperty is null) {
					throw new InvalidOperationException($"The type {typeof(TSingleton)} does not have a property named {singletonPropertyName}.");
				}

				if (singletonProperty.PropertyType != dependentProperty.PropertyType) {
					throw new InvalidOperationException($"The types of {derivedType.Name}.{dependentProperty.Name}" +
					                                    $"and {typeof(TSingleton)}.{singletonPropertyName} do not match.");
				}

				SingletonGetter.PropertyChanged += (_, args) =>
					PropertyChangedEventHandler(args, singletonPropertyName, dependentProperty.Name);
			}
		}
	}

	private void PropertyChangedEventHandler(PropertyChangedEventArgs args, string singletonProperty, string dependentPropertyName) {

		if (args.PropertyName == singletonProperty) {
			PropertyChanged?.Invoke(this, new(dependentPropertyName));
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;

}









public class Singleton : INotifyPropertyChanged {



	private string _Text = "";
	public string Text {
		get => _Text;
		set {
			_Text = value;
			OnPropertyChanged(nameof(Text));
		}
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}

public class Test : DependentBase<Singleton>, INotifyPropertyChanged {

	private static string _Text = "";
	[Dependent<Test, Singleton>(nameof(Singleton.Text))] public string Text {
		get => _Text;
		set {
			_Text = value;
			OnPropertyChanged(nameof(Text));
		}
	}


	protected override Singleton SingletonGetter { get; }



	public new event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}