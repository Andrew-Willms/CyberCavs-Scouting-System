using System;
using UtilitiesLibrary.MiscExtensions;
using Xunit;

namespace UtilitiesLibrary.Tests.MiscExtensions; 



public class IsDirectlyAssignableTo {

	private interface IFoo { }

	private class Foo : IFoo { }

	private class FooChild : Foo { }

	private class Bar { }

	private class Dim<T> { }



	public static object[][] NonGenericClassAssignableToSelfData => new[] {
		new object[] { typeof(Foo), typeof(Foo) },
		new object[] { new Foo().GetType(), new Foo().GetType() }
	};

	[Theory]
	[MemberData(nameof(NonGenericClassAssignableToSelfData))]
	public void NonGenericClassIsAssignableToSelf(Type type, Type targetType) {

		Assert.True(type.IsDirectlyAssignableTo(targetType));
	}



	public static object[][] UnrelatedClassesNotAssignableToEachOtherData => new[] {
		new object[] { typeof(Foo), typeof(Bar) },
		new object[] { new Foo().GetType(), new Bar().GetType() },
		new object[] { typeof(Foo), typeof(Dim<int>) },
		new object[] { new Foo().GetType(), new Dim<int>().GetType() }
	};

	[Theory]
	[MemberData(nameof(UnrelatedClassesNotAssignableToEachOtherData))]
	public void UnrelatedClassesNotAssignableToEachOther(Type type, Type targetType) {

		Assert.False(type.IsDirectlyAssignableTo(targetType));
	}



	public static object[][] NonGenericParentNotAssignableToChildData => new[] {
		new object[] { typeof(Foo), typeof(FooChild) },
		new object[] { new Foo().GetType(), new FooChild().GetType() }
	};

	[Theory]
	[MemberData(nameof(NonGenericParentNotAssignableToChildData))]
	public void NonGenericParentNotAssignableToChild(Type type, Type targetType) {

		Assert.False(type.IsDirectlyAssignableTo(targetType));
	}



	public static object[][] NonGenericChildNotAssignableToParentData => new[] {
		new object[] { typeof(FooChild), typeof(Foo) },
		new object[] { new FooChild().GetType(), new Foo().GetType() }
	};

	[Theory]
	[MemberData(nameof(NonGenericChildNotAssignableToParentData))]
	public void NonGenericChildNotAssignableToParent(Type type, Type targetType) {

		Assert.False(type.IsDirectlyAssignableTo(targetType));
	}



	public static object[][] ClosedGenericAssignableToOpenGenericData => new[] {
		new object[] { typeof(Dim<int>), typeof(Dim<>) },
		new object[] { new Dim<int>().GetType(), typeof(Dim<>) }
	};

	[Theory]
	[MemberData(nameof(ClosedGenericAssignableToOpenGenericData))]
	public void ClosedGenericAssignableToOpenGeneric(Type type, Type targetType) {

		Assert.True(type.IsDirectlyAssignableTo(targetType));
	}



	public static object[][] ClosedGenericNotAssignableToOtherClosedGenericData => new[] {
		new object[] { typeof(Dim<int>), typeof(Dim<float>) },
		new object[] { new Dim<int>().GetType(), new Dim<float>().GetType() }
	};

	[Theory]
	[MemberData(nameof(ClosedGenericNotAssignableToOtherClosedGenericData))]
	public void ClosedGenericNotAssignableToOtherClosedGeneric(Type type, Type targetType) {

		Assert.False(type.IsDirectlyAssignableTo(targetType));
	}



	public static object[][] OpenGenericNotAssignableToClosedGenericData => new[] {
		new object[] { typeof(Dim<>), typeof(Dim<int>) },
		new object[] { typeof(Dim<>), new Dim<int>().GetType() }
	};

	[Theory]
	[MemberData(nameof(OpenGenericNotAssignableToClosedGenericData))]
	public void OpenGenericNotAssignableToClosedGeneric(Type type, Type targetType) {

		Assert.False(type.IsDirectlyAssignableTo(targetType));
	}



	public static object[][] InterfaceNotAssignableToImplementingClassData => new[] {
		new object[] { typeof(IFoo), typeof(Foo) },
		new object[] { new Foo().GetType().GetInterface(nameof(IFoo))!, new Foo().GetType() }
	};

	[Theory]
	[MemberData(nameof(InterfaceNotAssignableToImplementingClassData))]
	public void InterfaceNotAssignableToImplementingClass(Type type, Type targetType) {

		Assert.False(type.IsDirectlyAssignableTo(targetType));
	}



	public static object[][] ClassNotAssignableToInterfaceItImplementsData => new[] {
		new object[] { typeof(Foo), typeof(IFoo) },
		new object[] { new Foo().GetType(), new Foo().GetType().GetInterface(nameof(IFoo))! },
	};

	[Theory]
	[MemberData(nameof(ClassNotAssignableToInterfaceItImplementsData))]
	public void ClassNotAssignableToInterfaceItImplements(Type type, Type targetType) {

		Assert.False(type.IsDirectlyAssignableTo(targetType));
	}

}