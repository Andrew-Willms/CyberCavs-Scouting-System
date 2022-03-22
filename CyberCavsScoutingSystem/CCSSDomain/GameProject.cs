using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSSDomain;

public class GameProject {

	// Do I even need this or should this be generated later?
	public Game Game = new();

	public GameEditingData EditingData = new();

	// Maybe have an edit history here too, work it out later.

	// Hash to check if the file hasn't been modified.

	// Project specific editor settings.
}

