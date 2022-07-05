using CCSSDomain.Game;

namespace CCSSDomain.Test;



public class GameProject {

	// Do I even need this or should this be generated later?
	public Game.Game Game = new();

	public GameEditingData EditingData = GameEditingData.GetDefaultEditingData();

	// Maybe have an edit history here too, work it out later.

	// Hash to check if the file hasn't been modified.

	// Project specific editor settings.
}

