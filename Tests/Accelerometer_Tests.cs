using Xamarin.Essentials;
using Xunit;

namespace Tests
{
    public class Accelerometer_Tests
    {
        [Fact]
        public void Accelerometer_Start() =>
            Assert.Throws<NotImplementedInReferenceAssemblyException>(() => Accelerometer.Stop());

        [Fact]
        public void Accelerometer_Stop() =>
            Assert.Throws<NotImplementedInReferenceAssemblyException>(() => Accelerometer.Start(SensorSpeed.Default));

        [Fact]
        public void Accelerometer_IsMonitoring() =>
            Assert.False(Accelerometer.IsMonitoring);

        [Fact]
        public void Accelerometer_IsSupported() =>
            Assert.Throws<NotImplementedInReferenceAssemblyException>(() => Accelerometer.IsSupported);

        [Theory]
        [InlineData(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, true)]
        [InlineData(0.0, 0.0, 0.0, 1.0, 0.0, 0.0, false)]
        [InlineData(0.0, 0.0, 0.0, 0.0, 1.0, 0.0, false)]
        [InlineData(0.0, 0.0, 0.0, 0.0, 0.0, 1.0, false)]
        public void Accelerometer_Comparison(
              float x1,
              float y1,
              float z1,
              float x2,
              float y2,
              float z2,
              bool equals)
        {
            var data1 = new AccelerometerData(x1, y1, z1);
            var data2 = new AccelerometerData(x2, y2, z2);
            if (equals)
            {
                Assert.True(data1.Equals(data2));
                Assert.True(data1 == data2);
                Assert.False(data1 != data2);
                Assert.Equal(data1, data2);
                Assert.Equal(data1.GetHashCode(), data2.GetHashCode());
            }
            else
            {
                Assert.False(data1.Equals(data2));
                Assert.False(data1 == data2);
                Assert.True(data1 != data2);
                Assert.NotEqual(data1, data2);
                Assert.NotEqual(data1.GetHashCode(), data2.GetHashCode());
            }
        }

        [Fact]
        public void InitialShaking()
        {
            var q = new AccelerometerQueue();
            Assert.False(q.IsShaking);
        }

        [Fact]
        public void ShakingTests()
        {
            var q = new AccelerometerQueue();
            q.Add(1000000000L, false);
            q.Add(1300000000L, false);
            q.Add(1600000000L, false);
            q.Add(1900000000L, false);
            Assert.False(q.IsShaking);

            // The oldest two entries will be removed.
            q.Add(2200000000L, true);
            q.Add(2500000000L, true);
            Assert.False(q.IsShaking);

            // Another entry should be removed, now 3 out of 4 are true.
            q.Add(2800000000L, true);
            Assert.True(q.IsShaking);

            q.Add(3100000000L, false);
            Assert.True(q.IsShaking);

            q.Add(3400000000L, false);
            Assert.False(q.IsShaking);
        }

        [Fact]
        public void ClearQueue()
        {
            var q = new AccelerometerQueue();
            q.Add(1000000000L, true);
            q.Add(1200000000L, true);
            q.Add(1400000000L, true);
            Assert.True(q.IsShaking);
            q.Clear();
            Assert.False(q.IsShaking);
        }
    }
}
