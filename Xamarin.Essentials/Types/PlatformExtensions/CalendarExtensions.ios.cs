using System;
using System.Drawing;
using Foundation;
using iOSSize = CoreGraphics.CGSize;

namespace Xamarin.Essentials
{
    public static partial class CalendarExtensions
    {
        // https://developer.apple.com/documentation/foundation/nsdate
        // NSDate minimum date is 2001/01/01
        static DateTime iosNSDateTimeSystemZeroPoint = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        static long ToEpochTime(this NSDate date) => (long)Math.Floor((Math.Abs(NSDate.FromTimeIntervalSince1970(0).SecondsSinceReferenceDate) + date.SecondsSinceReferenceDate) * 1000);

        public static NSDate ToNSDate(this DateTimeOffset date) => NSDate.FromTimeIntervalSinceReferenceDate((date.UtcDateTime - iosNSDateTimeSystemZeroPoint).TotalSeconds).AddSeconds(date.Offset.TotalSeconds);

        public static DateTimeOffset ToDateTimeOffset(this NSDate date) => DateTimeOffset.FromUnixTimeMilliseconds(date.ToEpochTime());
    }
}
