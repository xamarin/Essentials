using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Essentials;
using Xunit;

namespace Tests
{
    public class Recorder_Tests
    {
        public Recorder_Tests()
        {
            File.Delete(Samplefile);
        }

        string Samplefile => Path.Combine(
        Path.GetDirectoryName(typeof(Recorder_Tests).Assembly.Location),
        "sample.wav");

        [Fact]
        public void RecordedAudio_FinalizerDeletesFile()
        {
            using (var temp = File.Create(Samplefile))
            {
            }
            Create();
            GC.Collect(0, GCCollectionMode.Forced, blocking: true);

            GC.WaitForPendingFinalizers();
            GC.Collect(1, GCCollectionMode.Forced, blocking: true);

            GC.Collect();
            Assert.False(File.Exists(Samplefile));
        }

        [Fact]
        public void RecordedAudio_DisposeDeletesFile()
        {
            using (var temp = File.Create(Samplefile))
            {
            }

            using (var file = new RecordedAudio(Samplefile))
            {
            }

            Assert.False(File.Exists(Samplefile));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void Create()
        {
            new RecordedAudio(Samplefile);
        }
    }
}
