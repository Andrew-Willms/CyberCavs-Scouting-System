using System.ComponentModel;
using GameMakerWpf.AppManagement;

namespace GameMakerWpf.Views.Tabs;



public partial class PublishTabView : AppManagerDependent {

	public PublishTabView() {
		InitializeComponent();
	}



	protected override void OnPropertyChanged(string propertyName) {
		throw new System.NotImplementedException();
	}

	public override event PropertyChangedEventHandler? PropertyChanged;
}