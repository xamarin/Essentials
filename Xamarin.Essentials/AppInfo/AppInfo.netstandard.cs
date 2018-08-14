namespace Xamarin.Essentials
{
    public static partial class AppInfo
    {
        static string PlatformGetPackageName() => throw new NotImplementedInReferenceAssemblyException();

        static string PlatformGetName() => throw new NotImplementedInReferenceAssemblyException();

        static string PlatformGetVersionString() => throw new NotImplementedInReferenceAssemblyException();

        static string PlatformGetBuild() => throw new NotImplementedInReferenceAssemblyException();

        static void PlatformOpenSettings() => throw new NotImplementedInReferenceAssemblyException();

        static AppState PlatformState => throw new NotImplementedInReferenceAssemblyException();

        static void StartStateListeners() => throw new NotImplementedInReferenceAssemblyException();

        static void StopStateListeners() => throw new NotImplementedInReferenceAssemblyException();
    }
}
