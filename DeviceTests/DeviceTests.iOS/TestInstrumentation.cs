using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.DotNet.XHarness.TestRunners.Common;
using Microsoft.DotNet.XHarness.TestRunners.Xunit;
using Xamarin.Essentials;

namespace DeviceTests.iOS
{
    public static class TestInstrumentation
    {
        public static async Task StartAsync(string[] args)
        {
            Console.WriteLine($"ProcessorCount = {Environment.ProcessorCount}");
            Console.WriteLine($"Args: {string.Join(' ', args)}");

            var entryPoint = new TestsEntryPoint();
            entryPoint.TestsCompleted += (sender, results) =>
            {
                var message =
                    $"Tests run: {results.ExecutedTests} " +
                    $"Passed: {results.PassedTests} " +
                    $"Inconclusive: {results.InconclusiveTests} " +
                    $"Failed: {results.FailedTests} " +
                    $"Ignored: {results.SkippedTests}";
                Console.WriteLine(message);
            };

            await entryPoint.RunAsync();
            Console.WriteLine("\nDone.\n");
        }

        public class TestsEntryPoint : iOSApplicationEntryPoint
        {
            protected override bool LogExcludedTests => true;

            protected override int? MaxParallelThreads => Environment.ProcessorCount;

            protected override IDevice Device { get; } = new TestDevice();

            protected override IEnumerable<TestAssemblyInfo> GetTestAssemblies()
            {
                yield return new TestAssemblyInfo(Assembly.GetExecutingAssembly(), Assembly.GetExecutingAssembly().Location);
            }

            protected override void TerminateWithSuccess()
            {
            }

            protected override TestRunner GetTestRunner(LogWriter logWriter)
            {
                var testRunner = base.GetTestRunner(logWriter);
                testRunner.SkipCategories(Traits.GetSkipTraits());
                return testRunner;
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
