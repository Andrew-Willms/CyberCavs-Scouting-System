using System;
using System.Windows.Markup;

namespace WPFUtilities;



public class GenericObjectFactoryExtension : MarkupExtension {

	public Type Type { get; set; }
	public Type T { get; set; }

	public override object? ProvideValue(IServiceProvider serviceProvider) {
		Type genericType = Type.MakeGenericType(T);
		return Activator.CreateInstance(genericType);
	}

}

public class GenericObjectFactoryExtension2 : MarkupExtension {

	public Type Type { get; set; }
	public Type T1 { get; set; }
	public Type T2 { get; set; }

	public override object? ProvideValue(IServiceProvider serviceProvider) {
		Type genericType = Type.MakeGenericType(T1, T2);
		return Activator.CreateInstance(genericType);
	}

}