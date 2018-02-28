using System.Runtime.CompilerServices;

namespace Microsoft.Caboodle
{
	public static class PreferencesExtensions
	{
		public static string Get(this Preferences preferences, string defaultValue, [CallerMemberName]string key = null) =>
			preferences.Get(key, defaultValue);
		public static bool Get(this Preferences preferences, bool defaultValue, [CallerMemberName]string key = null) =>
			preferences.Get(key, defaultValue);
		public static int Get(this Preferences preferences, int defaultValue, [CallerMemberName]string key = null) =>
			preferences.Get(key, defaultValue);
		public static double Get(this Preferences preferences, double defaultValue, [CallerMemberName]string key = null) =>
			preferences.Get(key, defaultValue);
		public static float Get(this Preferences preferences, float defaultValue, [CallerMemberName]string key = null) =>
			preferences.Get(key, defaultValue);
		public static long Get(this Preferences preferences, long defaultValue, [CallerMemberName]string key = null) =>
			preferences.Get(key, defaultValue);

		public static string Set(this Preferences preferences, string value, [CallerMemberName]string key = null) =>
			preferences.Set(key, value);
		public static bool Set(this Preferences preferences, bool value, [CallerMemberName]string key = null) =>
			preferences.Set(key, value);
		public static int Set(this Preferences preferences, int value, [CallerMemberName]string key = null) =>
			preferences.Set(key, value);
		public static double Set(this Preferences preferences, double value, [CallerMemberName]string key = null) =>
			preferences.Set(key, value);
		public static float Set(this Preferences preferences, float value, [CallerMemberName]string key = null) =>
			preferences.Set(key, value);
		public static long Set(this Preferences preferences, long value, [CallerMemberName]string key = null) =>
			preferences.Set(key, value);
	}
}
