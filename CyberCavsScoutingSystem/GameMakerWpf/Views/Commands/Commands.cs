using System.Windows.Input;

namespace GameMakerWpf.Views.Commands; 



public static class Commands {

	public static readonly RoutedUICommand SaveAs = new(
		text: nameof(SaveAs),
		name: nameof(SaveAs),
		ownerType: typeof(Commands),
		inputGestures: new() {
			new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift)
		}
	);

	public static readonly RoutedUICommand Publish = new(
		text: nameof(Publish),
		name: nameof(Publish),
		ownerType: typeof(Commands),
		inputGestures: new() {
			new KeyGesture(Key.P, ModifierKeys.Control)
		}
	);

}