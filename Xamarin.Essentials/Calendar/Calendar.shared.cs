using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Calendar
    {
        static TimeSpan defaultStartTimeFromNow = TimeSpan.Zero;

        static TimeSpan defaultEndTimeFromStartTime = TimeSpan.FromDays(14);

        public static Task<IEnumerable<DeviceCalendar>> GetCalendarsAsync() => PlatformGetCalendarsAsync();

        public static Task<IEnumerable<DeviceEvent>> GetEventsAsync(string calendarId = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null) => PlatformGetEventsAsync(calendarId, startDate, endDate);

        public static Task<DeviceEvent> GetEventByIdAsync(string eventId) => PlatformGetEventByIdAsync(eventId);
    }
}
