using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Android.App;
using Android.OS;
using Android.Runtime;
using Microsoft.DotNet.XHarness.TestRunners.Common;
using Microsoft.DotNet.XHarness.TestRunners.Xunit;
using Xamarin.Essentials;

namespace DeviceTests.Droid
{
    [Instrumentation(Name = "com.xamarin.essentials.devicetests.TestInstrumentation")]
    public class TestInstrumentation : Instrumentation
    {
        string resultsFileName;

        protected TestInstrumentation()
        {
        }

        protected TestInstrumentation(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }

        public override void OnCreate(Bundle arguments)
        {
            base.OnCreate(arguments);

            resultsFileName = arguments.GetString("results-file-name", "TestResults.xml");

            Start();
        }

        public override async void OnStart()
        {
            base.OnStart();

            var bundle = new Bundle();

            var runner = new TestRunner(resultsFileName);
            runner.TestsCompleted += (sender, results) =>
            {
                var message =
                    $"Tests run: {results.ExecutedTests} " +
                    $"Passed: {results.PassedTests} " +
                    $"Inconclusive: {results.InconclusiveTests} " +
                    $"Failed: {results.FailedTests} " +
                    $"Ignored: {results.SkippedTests}";
                bundle.PutString("test-execution-summary", message);

                bundle.PutLong("return-code", results.FailedTests == 0 ? 0 : 1);
            };

            await runner.RunAsync();

            // if (File.Exists(runner.TestsResultsFinalPath))
            //    bundle.PutString("test-results-path", runner.TestsResultsFinalPath);

            if (bundle.GetLong("return-code", -1) == -1)
                bundle.PutLong("return-code", 1);

            Finish(Result.Ok, bundle);
        }

        class TestRunner : AndroidApplicationEntryPoint
        {
            readonly string resultsPath;

            public TestRunner(string resultsFileName)
            {
                var docsDir = Application.Context.GetExternalFilesDir(null)?.AbsolutePath ?? FileSystem.AppDataDirectory;
                if (!Directory.Exists(docsDir))
                    Directory.CreateDirectory(docsDir);

                resultsPath = Path.Combine(docsDir, resultsFileName);
            }

            public override TextWriter Logger => null;

            public override string TestsResultsFinalPath => resultsPath;

            protected override int? MaxParallelThreads => System.Environment.ProcessorCount;

            protected override IDevice Device { get; } = new TestDevice();

            protected override IEnumerable<TestAssemblyInfo> GetTestAssemblies()
            {
                yield return new TestAssemblyInfo(Assembly.GetExecutingAssembly(), Assembly.GetExecutingAssembly().Location);
                yield return new TestAssemblyInfo(typeof(Battery_Tests).Assembly, typeof(Battery_Tests).Assembly.Location);
            }

            protected override void TerminateWithSuccess()
            {
            }
        }

        class TestDevice : IDevice
        {
            public string BundleIdentifier => AppInfo.PackageName;

            public string UniqueIdentifier => Guid.NewGuid().ToString("N");

            public string Name => DeviceInfo.Name;

            public string Model => DeviceInfo.Model;

            public string SystemName => DeviceInfo.Platform.ToString();

            public string SystemVersion => DeviceInfo.VersionString;

            public string Locale => CultureInfo.CurrentCulture.Name;
        }
    }
}
