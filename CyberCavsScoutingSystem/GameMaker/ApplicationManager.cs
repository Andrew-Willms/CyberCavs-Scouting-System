using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCSS_SharedClasses;

namespace GameMaker {

	public static class ApplicationManager {

		private static GameProject GameProject;

		public static bool ApplicationStartup() {

			GameProject = new();

			return true;
		}

		public static void SaveGameProject() {

		}

		public static void LoadGameProject() {

		}



	}

}
