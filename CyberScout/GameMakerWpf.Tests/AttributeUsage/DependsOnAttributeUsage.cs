using System;
using System.Diagnostics;
using System.Reflection;
using GameMakerWpf.AppManagement;
using UtilitiesLibrary.MiscExtensions;
using WPFUtilities;
using Xunit;

namespace GameMakerWpf.Tests.AttributeUsage; 



public class DependsOnAttributeUsage {

	[Fact]
	public void DependsOnAttributeOnlyUsedInDependentControl() {

		Assembly? gameMakerAssembly = Assembly.GetAssembly(typeof(AppManager));

		Assert.NotNull(gameMakerAssembly);
		gameMakerAssembly = gameMakerAssembly ?? throw new UnreachableException();

		foreach (Type type in gameMakerAssembly.GetTypes()) {

			foreach (PropertyInfo propertyInfo in type.GetProperties()) {

				if (propertyInfo.GetCustomAttributes(typeof(DependsOnAttribute), true).Length == 0) {
					continue;
				}

				bool isAssignableToDependentControl = type.Inherits(typeof(DependentControl<>));

				Assert.True(isAssignableToDependentControl,
					$"The type \"{type}\" has a the property \"{propertyInfo.Name}\" which is decorated with an attribute of type \"{typeof(DependsOnAttribute)}\" " +
					$"but \"{type}\" does not inherit from {typeof(DependentControl<>)}. Only classes that inherit from {typeof(DependentControl<>)} should " +
					$"have properties decorated with {nameof(DependsOnAttribute)}s.");
			}
		}
	}

}