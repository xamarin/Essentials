using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;

namespace Microsoft.Caboodle
{
	public static partial class FileSystem
	{
		private static string cache;
		private static string appData;
		private static string userData;

		public static string CacheDirectory
			=> cache ?? (cache = GetDirectory(NSSearchPathDirectory.CachesDirectory));

		public static string AppDataDirectory
			=> appData ?? (appData = GetDirectory(NSSearchPathDirectory.ApplicationSupportDirectory));

		public static string UserDataDirectory
			=> userData ?? (userData = GetDirectory(NSSearchPathDirectory.DocumentDirectory));

		public static Task<Stream> OpenAppPackageFileAsync(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException(nameof(filename));

			filename = filename.Replace("\\", "/");
			var file = Path.Combine(NSBundle.MainBundle.BundlePath, filename);
			return Task.FromResult((Stream)File.OpenRead(file));
		}

		static string GetDirectory(NSSearchPathDirectory directory)
		{
			var dirs = NSSearchPath.GetDirectories(directory, NSSearchPathDomain.User);
			if (dirs == null || dirs.Length == 0)
			{
				throw new ArgumentException(nameof(directory));
			}
			return dirs[0];
		}
	}
}
