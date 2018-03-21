using System.Threading.Tasks;
using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.DeviceTests
{
    public class WakeLock_Tests
    {
        [Fact]
        public Task WakeLock_Locks()
        {
            return Utils.OnMainThread(() =>
            {
                Assert.False(WakeLock.IsActive);

                WakeLock.RequestActive();
                Assert.True(WakeLock.IsActive);

                WakeLock.RequestRelease();
                Assert.False(WakeLock.IsActive);
            });
        }

        [Fact]
        public Task WakeLock_Unlocks_Without_Locking()
        {
            return Utils.OnMainThread(() =>
            {
                Assert.False(WakeLock.IsActive);

                WakeLock.RequestRelease();
                Assert.False(WakeLock.IsActive);
            });
        }

        [Fact]
        public Task WakeLock_Locks_Only_Once()
        {
            return Utils.OnMainThread(() =>
            {
                Assert.False(WakeLock.IsActive);

                WakeLock.RequestActive();
                Assert.True(WakeLock.IsActive);
                WakeLock.RequestActive();
                Assert.True(WakeLock.IsActive);

                WakeLock.RequestRelease();
                Assert.False(WakeLock.IsActive);
            });
        }
    }
}
