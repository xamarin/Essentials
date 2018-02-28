using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Microsoft.Caboodle
{
	public static partial class FileSystem
	{
		private static string cache;
		private static string appData;
		private static string userData;

		public static string CacheDirectory
			=> cache ?? (cache = ApplicationData.Current.LocalCacheFolder.Path);

		public static string AppDataDirectory
			=> appData ?? (appData = ApplicationData.Current.LocalFolder.Path);

		public static string UserDataDirectory
			=> userData ?? (userData = ApplicationData.Current.LocalFolder.Path);

		public static Task<Stream> OpenAppBundleFileAsync(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException(nameof(filename));

			return Package.Current.InstalledLocation.OpenStreamForReadAsync(filename);
		}
	}
}
