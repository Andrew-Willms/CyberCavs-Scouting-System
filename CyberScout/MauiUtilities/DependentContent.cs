//using System.ComponentModel;
//using System.Reflection;

//namespace MauiUtilities;



//[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
//public class DependsOnAttribute : Attribute {

//	public string IndependentPropertyName { get; }

//	public DependsOnAttribute(string independentPropertyName) {

//		IndependentPropertyName = independentPropertyName;
//	}

//}



//public abstract class DependentView<TSingleton> : ContentView, INotifyPropertyChanged
//	where TSingleton : INotifyPropertyChanged {

//	protected abstract TSingleton SingletonGetter { get; }

//	protected DependentView() {

//		IEnumerable<PropertyInfo> propertiesOfSingleton = typeof(TSingleton).GetProperties();

//		IEnumerable<PropertyInfo> dependentProperties = GetType().GetProperties()
//			.Where(x => x.GetCustomAttributes(typeof(DependsOnAttribute), true).Any());

//		foreach (PropertyInfo dependentProperty in dependentProperties) {

//			IEnumerable<DependsOnAttribute> dependentAttributes = dependentProperty.GetCustomAttributes<DependsOnAttribute>();

//			foreach (DependsOnAttribute dependentAttribute in dependentAttributes) {

//				string independentPropertyName = dependentAttribute.IndependentPropertyName;
//				PropertyInfo? singletonProperty = propertiesOfSingleton.FirstOrDefault(x => x.Name == independentPropertyName);

//				if (singletonProperty is null) {
//					throw new InvalidOperationException($"The type {typeof(TSingleton)} does not have a property named {independentPropertyName}.");
//				}

//				// This call to a abstract member is fine since the member is auto initialized (before constructor).
//				// ReSharper disable once VirtualMemberCallInConstructor 
//				SingletonGetter.PropertyChanged += (_, args) => PropertyChangedEventHandler(args, independentPropertyName, dependentProperty.Name);
//			}
//		}
//	}

//	private void PropertyChangedEventHandler(PropertyChangedEventArgs args, string independentPropertyName, string dependentPropertyName) {

//		if (args.PropertyName == independentPropertyName) {
//			OnPropertyChanged(dependentPropertyName);
//		}
//	}

//}



//public abstract class DependentPage<TSingleton> : ContentView, INotifyPropertyChanged
//	where TSingleton : INotifyPropertyChanged {

//	protected abstract TSingleton SingletonGetter { get; }

//	protected DependentPage() {

//		IEnumerable<PropertyInfo> propertiesOfSingleton = typeof(TSingleton).GetProperties();

//		IEnumerable<PropertyInfo> dependentProperties = GetType().GetProperties()
//			.Where(x => x.GetCustomAttributes(typeof(DependsOnAttribute), true).Any());

//		foreach (PropertyInfo dependentProperty in dependentProperties) {

//			IEnumerable<DependsOnAttribute> dependentAttributes = dependentProperty.GetCustomAttributes<DependsOnAttribute>();

//			foreach (DependsOnAttribute dependentAttribute in dependentAttributes) {

//				string independentPropertyName = dependentAttribute.IndependentPropertyName;
//				PropertyInfo? singletonProperty = propertiesOfSingleton.FirstOrDefault(x => x.Name == independentPropertyName);

//				if (singletonProperty is null) {
//					throw new InvalidOperationException($"The type {typeof(TSingleton)} does not have a property named {independentPropertyName}.");
//				}

//				// This call to a abstract member is fine since the member is auto initialized (before constructor).
//				// ReSharper disable once VirtualMemberCallInConstructor 
//				SingletonGetter.PropertyChanged += (_, args) => PropertyChangedEventHandler(args, independentPropertyName, dependentProperty.Name);
//			}
//		}
//	}

//	private void PropertyChangedEventHandler(PropertyChangedEventArgs args, string independentPropertyName, string dependentPropertyName) {

//		if (args.PropertyName == independentPropertyName) {
//			OnPropertyChanged(dependentPropertyName);
//		}
//	}

//}