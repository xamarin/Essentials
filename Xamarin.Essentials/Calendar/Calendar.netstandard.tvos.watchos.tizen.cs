using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Calendar
    {
        static Task<IEnumerable<DeviceCalendar>> PlatformGetCalendarsAsync() => throw new NotImplementedException();

        static Task<IEnumerable<DeviceEvent>> PlatformGetEventsAsync(string calendarId = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null) => throw new NotImplementedException();

        static Task<DeviceEvent> PlatformGetEventByIdAsync(string eventId) => throw new NotImplementedException();
    }
}
