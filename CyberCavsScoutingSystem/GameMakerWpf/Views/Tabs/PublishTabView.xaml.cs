using System.ComponentModel;
using System.Windows;
using GameMakerWpf.AppManagement;
using GameMakerWpf.Domain.Editors;

namespace GameMakerWpf.Views.Tabs;



public partial class PublishTabView : AppManagerDependent {

	// These can't be static or PropertyChanged events on them won't work.
	private GameEditor GameEditor => App.Manager.GameEditor;

	public bool GameIsValid => GameEditor.IsValid;



	public PublishTabView() {

		DataContext = this;

		InitializeComponent();

		GameEditor.AnythingChanged.Subscribe(() => OnPropertyChanged(nameof(GameIsValid)));
	}



	public override event PropertyChangedEventHandler? PropertyChanged;

	protected override void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

	private void PublishButton_OnClick(object sender, RoutedEventArgs e) {
		App.Manager.Publish();
	}

}