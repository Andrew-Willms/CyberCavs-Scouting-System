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

}