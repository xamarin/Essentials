var TARGET = Argument("t", Argument("target", "ci"));

var NUGET_VERSION = EnvironmentVariable("NUGET_VERSION") ?? "1.0.0";
var GIT_SHA = Argument("gitSha", EnvironmentVariable("GIT_SHA") ?? "");
var GIT_BRANCH_NAME = Argument("gitBranch", EnvironmentVariable("GIT_BRANCH_NAME") ?? "");

var RESTORE_CONFIG = MakeAbsolute(new FilePath("./devopsnuget.config")).FullPath;

Task("prepare")
	.Does(() =>
{
	// Update .csproj nuget versions
	XmlPoke("./Xamarin.Essentials/Xamarin.Essentials.csproj", "/Project/PropertyGroup/PackageVersion", NUGET_VERSION);
});

Task("libs")
	.IsDependentOn("prepare")
	.Does(() =>
{
	MSBuild("./Xamarin.Essentials/Xamarin.Essentials.csproj", new MSBuildSettings()
		.EnableBinaryLogger("./output/binlogs/libs.binlog")
		.SetConfiguration("Release")
		.WithProperty("RestoreConfigFile", RESTORE_CONFIG)
		.WithRestore());
});

Task("nugets")
	.IsDependentOn("prepare")
	.IsDependentOn("libs")
	.IsDependentOn("docs")
	.Does(() =>
{
	MSBuild("./Xamarin.Essentials/Xamarin.Essentials.csproj", new MSBuildSettings()
		.EnableBinaryLogger("./output/binlogs/nugets.binlog")
		.SetConfiguration("Release")
		.WithRestore()
		.WithProperty("PackageOutputPath", MakeAbsolute(new FilePath("./output/")).FullPath)
		.WithProperty("RestoreConfigFile", RESTORE_CONFIG)
		.WithTarget("Pack"));
});

Task("tests")
	.IsDependentOn("libs")
	.Does(() =>
{
	var failed = 0;

	foreach (var csproj in GetFiles("./Tests/**/*.csproj")) {
		try {
			DotNetCoreTest(csproj.FullPath, new DotNetCoreTestSettings {
				Configuration = "Release",
				Logger = $"trx;LogFileName={csproj.GetFilenameWithoutExtension()}.trx",
				EnvironmentVariables = new Dictionary<string, string> {
					{ "RestoreConfigFile", RESTORE_CONFIG }
				}
			});
		} catch (Exception) {
			failed++;
		}
	}

	var output = $"./output/test-results/";
	EnsureDirectoryExists(output);
	CopyFiles($"./tests/**/TestResults/*.trx", output);

	if (failed > 0)
		throw new Exception($"{failed} tests have failed.");
});

Task("samples")
	.IsDependentOn("nugets")
	.Does(() =>
{
	MSBuild("./Xamarin.Essentials.sln", new MSBuildSettings()
		.EnableBinaryLogger("./output/binlogs/samples.binlog")
		.SetConfiguration("Release")
		.WithProperty("RestoreConfigFile", RESTORE_CONFIG)
		.WithRestore());
});

Task("docs")
	.IsDependentOn("libs")
	.WithCriteria(IsRunningOnWindows())
	.Does(() =>
{
	MSBuild("./Xamarin.Essentials/Xamarin.Essentials.csproj", new MSBuildSettings()
		.EnableBinaryLogger("./output/binlogs/docs.binlog")
		.SetConfiguration("Release")
		.WithRestore()
		.WithProperty("RestoreConfigFile", RESTORE_CONFIG)
		.WithTarget("mdocupdatedocs"));
});

Task("ci")
	.IsDependentOn("libs")
	.IsDependentOn("nugets")
	.IsDependentOn("tests")
	.IsDependentOn("samples");

RunTarget(TARGET);
