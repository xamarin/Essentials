namespace Xamarin.Essentials
{
    public static partial class Preferences
    {
        private static bool PlatformContainsKey(string key, string sharedName) =>
            throw new NotImplementedInReferenceAssemblyException();

        private static void PlatformRemove(string key, string sharedName) =>
            throw new NotImplementedInReferenceAssemblyException();

        private static void PlatformClear(string sharedName) =>
            throw new NotImplementedInReferenceAssemblyException();

        private static void PlatformSet<T>(string key, T value, string sharedName) =>
            throw new NotImplementedInReferenceAssemblyException();

        private static T PlatformGet<T>(string key, T defaultValue, string sharedName) =>
            throw new NotImplementedInReferenceAssemblyException();
    }
}
