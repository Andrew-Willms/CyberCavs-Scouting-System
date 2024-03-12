using System.IO;
using System.Threading.Tasks;
using System;

namespace UtilitiesLibrary.MiscExtensions; 



public static class FileExtensions {

	// I guess this isn't an extension, oops.
	public static async Task<bool> EnsureFileExists(string path, string contentToAddIfCreatingFile = "") {

		bool fileExists = File.Exists(path);

		if (fileExists) {
			return true;
		}

		try {
			Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new ArgumentException());
			await File.WriteAllTextAsync(path, contentToAddIfCreatingFile);
		} catch {
			return false;
		}

		return true;
	}

}