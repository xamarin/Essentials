using Xamarin.Essentials;
using Xunit;

namespace Tests
{
    public class DeviceInfo_Tests
    {
        [Fact]
        public void StorageExtensions_PercentageExtensions()
        {
            var fullStorage = new StorageInfo(1000ul, 0, 1000ul, StorageType.Unknown);
            var emptyStorage = new StorageInfo(0, 0, 0, StorageType.Unknown);
            var bakersRoundingStorage = new StorageInfo(1000ul, 5ul, 0, StorageType.Unknown);
            Assert.Equal(0, fullStorage.FreePercentage());
            Assert.Equal(0, emptyStorage.FreePercentage());
            Assert.Equal(1, bakersRoundingStorage.FreePercentage());
        }
    }
}
