using GameMakerWpf.AppManagement;
using GameMakerWpf.Domain.Editors;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary.Results;
using Xunit;

namespace GameMakerWpf.Tests.Result; 



public class Results {

	[Fact]
	public void AllResultNestedClassesImplementErrorOrSuccess() {

		Assembly? gameMakerAssembly = Assembly.GetAssembly(typeof(AppManager));

		Assert.NotNull(gameMakerAssembly);
		gameMakerAssembly = gameMakerAssembly ?? throw new UnreachableException();

		foreach (Type type in gameMakerAssembly
			         .GetTypes()
			         .Where(x => x.IsInterface)
			         .Where(x => x.IsAssignableTo(typeof(IResult)))
			         .SelectMany(x => x.GetNestedTypes()).Where(x => x.IsClass)) {

			if (!type.IsAssignableTo(typeof(IResult))) {

				Assert.True(false, $"The type '{type}' is nested within an interface that implements '{typeof(IResult)}' " +
				                   $"but does not itself implement '{typeof(IResult)}'.");
			}

			if (!type.IsAssignableTo(typeof(IResult.Error)) && !type.IsAssignableTo(typeof(IResult.Success))) {

				Assert.True(false, $"The type '{type}' implements '{typeof(IResult)}' but does not inherit from " +
				                   $"'{typeof(IResult.Error)}' or '{typeof(IResult.Success)}'.");
			}
		}

		Trace.WriteLine(typeof(IEditorToGameSpecificationResult<Game>.Success).IsAssignableTo(typeof(IEditorToGameSpecificationResult<Game>))); // true
		Trace.WriteLine(typeof(IEditorToGameSpecificationResult<Game>.Success).IsAssignableTo(typeof(IResult<Game>))); // true
		Trace.WriteLine(typeof(IEditorToGameSpecificationResult<Game>.Success).IsAssignableTo(typeof(IResult<>))); // false
		Trace.WriteLine(typeof(IEditorToGameSpecificationResult<Game>.Success).GetGenericTypeDefinition().IsAssignableTo(typeof(IResult<>))); // false
		Trace.WriteLine(typeof(IEditorToGameSpecificationResult<Game>.Success).IsAssignableTo(typeof(IResult<Game>.Success))); // true
		Trace.WriteLine(typeof(IEditorToGameSpecificationResult<Game>.Success).IsAssignableTo(typeof(IResult<>.Success))); // false
		Trace.WriteLine(typeof(IEditorToGameSpecificationResult<Game>.Success).GetGenericTypeDefinition().IsAssignableTo(typeof(IResult<>.Success))); // false

		Trace.WriteLine(typeof(IEditorToGameSpecificationResult<Game>.Success));
		Trace.WriteLine(typeof(IEditorToGameSpecificationResult<Game>.Success).GetGenericTypeDefinition());
		Trace.WriteLine(typeof(IEditorToGameSpecificationResult<Game>.Success).GetGenericTypeDefinition().GetGenericTypeDefinition());

		foreach (Type type in gameMakerAssembly
			         .GetTypes()
			         .Where(x => x.IsInterface)
			         .Where(x => !x.IsAssignableTo(typeof(IResult)))
			         .Where(x => x.GetInterfaces().Select(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IResult<>)).Any())
			         .SelectMany(x => x.GetNestedTypes()).Where(x => x.IsClass)) {

			if (!type.Inherits(typeof(IResult<>))) {

				Assert.True(false, $"The type '{type}' is nested within an interface that implements '{typeof(IResult<>)}' " +
				                   $"but does not itself implement '{typeof(IResult<>)}'.");
			}

			if (!type.IsAssignableTo(typeof(IResult<>.Error)) && !type.IsAssignableTo(typeof(IResult<>.Success))) {

				Assert.True(false, $"The type '{type}' implements '{typeof(IResult<>)}' but does not inherit from " +
				                   $"'{typeof(IResult<>.Error)}' or '{typeof(IResult<>.Success)}'.");
			}
		}
	}

}