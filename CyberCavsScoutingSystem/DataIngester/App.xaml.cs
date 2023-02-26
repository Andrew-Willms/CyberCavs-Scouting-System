using System;
using CCSSDomain.GameSpecification;
using Microsoft.Maui.Controls;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;

namespace DataIngester;



public partial class App : Application {

	public static GameSpec GameSpecification { get; } = (GameSpec.Create(
		"Test",
		1,
		"Test Description",
		new(1, 0, 0),
		3,
		2,
		ReadOnlyList.Empty,
		ReadOnlyList.Empty,
		ReadOnlyList.Empty,
		ReadOnlyList.Empty,
		ReadOnlyList.Empty,
		ReadOnlyList.Empty) as IResult<GameSpec>.Success)?.Value ?? throw new InvalidOperationException();

	public App() {
		InitializeComponent();

		MainPage = new AppShell();
	}

}