using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Calendars
    {
        static Task<IEnumerable<Calendar>> PlatformGetCalendarsAsync() => throw ExceptionUtils.NotSupportedOrImplementedException;

        static Task<IEnumerable<CalendarEvent>> PlatformGetEventsAsync(string calendarId = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null) => throw ExceptionUtils.NotSupportedOrImplementedException;

        static Task<CalendarEvent> PlatformGetEventByIdAsync(string eventId) => throw ExceptionUtils.NotSupportedOrImplementedException;
    }
}
