﻿using System.ComponentModel;
using CCSSDomain.GameSpecification;
using GameMakerWpf.AppManagement;
using GameMakerWpf.Domain;
using GameMakerWpf.Domain.Editors;
using Microsoft.Extensions.DependencyInjection;
using UtilitiesLibrary.Validation.Inputs;
using WPFUtilities;

namespace GameMakerWpf.Views.Tabs;



public partial class MetaTabView : AppManagerDependent, INotifyPropertyChanged {

	// These can't be static or PropertyChanged events on them won't work.
	private GameEditor GameEditor => App.ServiceProvider.GetRequiredService<IAppManager>().GameEditor;

	[DependsOn(nameof(AppManager.GameEditor))]
	public SingleInput<string, string, ErrorSeverity> GameName => GameEditor.Name;

	[DependsOn(nameof(AppManager.GameEditor))]
	public SingleInput<string, string, ErrorSeverity> Description => GameEditor.Description;

	[DependsOn(nameof(AppManager.GameEditor))]
	public SingleInput<int, string, ErrorSeverity> Year => GameEditor.Year;

	[DependsOn(nameof(AppManager.GameEditor))]
	public MultiInput<Version, ErrorSeverity, uint, uint, uint, string> Version => GameEditor.Version;

	[DependsOn(nameof(AppManager.GameEditor))]
	public SingleInput<uint, string, ErrorSeverity> VersionMajorNumber => GameEditor.VersionMajorNumber;

	[DependsOn(nameof(AppManager.GameEditor))]
	public SingleInput<uint, string, ErrorSeverity> VersionMinorNumber => GameEditor.VersionMinorNumber;

	[DependsOn(nameof(AppManager.GameEditor))]
	public SingleInput<uint, string, ErrorSeverity> VersionPatchNumber => GameEditor.VersionPatchNumber;

	[DependsOn(nameof(AppManager.GameEditor))]
	public SingleInput<string, string, ErrorSeverity> VersionDescription => GameEditor.VersionDescription;


	public MetaTabView() {

		DataContext = this;

		InitializeComponent();
	}



	public override event PropertyChangedEventHandler? PropertyChanged;

	protected override void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}