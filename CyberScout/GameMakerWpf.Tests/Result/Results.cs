using GameMakerWpf.AppManagement;
using System;
using System.Linq;
using System.Reflection;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary.Results;
using Xunit;

namespace GameMakerWpf.Tests.Result; 



public class Results {

	private static Assembly GameMakerAssembly { get; } = 
		Assembly.GetAssembly(typeof(AppManager)) ?? throw new("GameMaker assembly not found.");

	[Fact]
	public void AllNestedClassesInResultInterfacesInheritTheContainingResultInterface() {

		foreach (Type containingResultInterface in GameMakerAssembly.GetInterfaces().Where(x => x.Implements(typeof(IResult)))) {

			foreach (Type resultOption in containingResultInterface.GetNestedClasses()) {

				Assert.True(resultOption.IsAssignableTo(containingResultInterface),
					$"The type '{resultOption}' is nested within a result interface but does not implement that result interface.");
			}
		}
	}

	[Fact]
	public void AllNestedClassesInValueResultInterfacesInheritTheContainingResultInterface() {

		foreach (Type containingResultInterface in GameMakerAssembly.GetInterfaces().Where(x => x.Implements(typeof(IResult<>)))) {

			foreach (Type resultOption in containingResultInterface.GetNestedClasses()) {

				Assert.True(resultOption.Implements(containingResultInterface),
					$"The type '{resultOption}' is nested within a result interface but does not implement that result interface.");
			}
		}
	}



	[Fact]
	public void AllResultNestedClassesImplementErrorOrSuccess() {

		foreach (Type containingResultInterface in GameMakerAssembly.GetInterfaces().Where(x => x.Implements(typeof(IResult)))) {

			foreach (Type resultOption in containingResultInterface.GetNestedClasses()) {

				Assert.True(resultOption.IsAssignableTo(typeof(IResult.Success)) || resultOption.IsAssignableTo(typeof(IResult.Error)),
					$"The type '{resultOption}' is nested within a result interface but does not inherit from " +
					$"'{typeof(IResult.Error)}' or '{typeof(IResult.Success)}'.");
			}
		}
	}


	[Fact]
	public void AllValueResultNestedClassesImplementErrorOrSuccess() {

		foreach (Type containingResultInterface in GameMakerAssembly.GetInterfaces().Where(x => x.Implements(typeof(IResult<>)))) {

			foreach (Type resultOption in containingResultInterface.GetNestedClasses()) {

				Type resultInterface =
					resultOption.GetInterfaces().FirstOrDefault(x => x.IsDirectlyAssignableTo(typeof(IResult<>)))
					?? throw new($"The type '{resultOption}' is nested within a result interface but does not implement that result interface.");

				Type successType = resultInterface.GetNestedClasses().First(x => x.IsDirectlyAssignableTo(typeof(IResult<>.Success)));
				Type errorType = resultInterface.GetNestedClasses().First(x => x.IsDirectlyAssignableTo(typeof(IResult<>.Error)));

				Assert.True(resultOption.Inherits(successType) || resultOption.Inherits(errorType),
					$"The type '{resultOption}' is nested within a result interface but does not inherit from " +
					$"'{errorType}' or '{successType}'.");
			}
		}
	}

}