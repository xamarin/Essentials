using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Calendars
    {
        static TimeSpan defaultStartTimeFromNow = TimeSpan.Zero;

        static TimeSpan defaultEndTimeFromStartTime = TimeSpan.FromDays(14);

        public static Task<IEnumerable<Calendar>> GetCalendarsAsync() => PlatformGetCalendarsAsync();

        public static Task<IEnumerable<CalendarEvent>> GetEventsAsync(string calendarId = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null) => PlatformGetEventsAsync(calendarId, startDate, endDate);

        public static Task<CalendarEvent> GetEventByIdAsync(string eventId) => PlatformGetEventByIdAsync(eventId);
    }
}
