using Microsoft.Caboodle;
using RuntimeDeviceType = Microsoft.Caboodle.DeviceType;

namespace Caboodle.DeviceTests
{
    internal static class Traits
    {
        public const string DeviceType = "DeviceType";

        internal static class DeviceTypes
        {
            public const string Physical = "Physical";
            public const string Virtual = "Virtual";

            internal static string ToExclude =>
                DeviceInfo.DeviceType == RuntimeDeviceType.Physical ? Virtual : Physical;
        }
    }
}
