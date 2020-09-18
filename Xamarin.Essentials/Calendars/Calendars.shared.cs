using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class Calendars
    {
        static readonly TimeSpan defaultStartTimeFromNow = TimeSpan.Zero;
        static readonly TimeSpan defaultEndTimeFromStartTime = TimeSpan.FromDays(14);

        public static async Task<IEnumerable<Calendar>> GetCalendarsAsync()
        {
            await Permissions.RequestAsync<Permissions.CalendarRead>();

            return await PlatformGetCalendarsAsync();
        }

        public static async Task<Calendar> GetCalendarAsync(string calendarId)
        {
            if (calendarId == null)
                throw new ArgumentNullException(nameof(calendarId));

            await Permissions.RequestAsync<Permissions.CalendarRead>();

            return await PlatformGetCalendarAsync(calendarId);
        }

        public static async Task<IEnumerable<CalendarEvent>> GetEventsAsync(string calendarId = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {
            await Permissions.RequestAsync<Permissions.CalendarRead>();

            return await PlatformGetEventsAsync(calendarId, startDate, endDate);
        }

        public static async Task<CalendarEvent> GetEvent(string eventId)
        {
            if (eventId == null)
                throw new ArgumentNullException(nameof(eventId));

            await Permissions.RequestAsync<Permissions.CalendarRead>();

            return await PlatformGetEventAsync(eventId);
        }

        static ArgumentException InvalidCalendar(string calendarId) =>
            new ArgumentException($"No calendar exists with the Id {calendarId}.", nameof(calendarId));

        static ArgumentException InvalidEvent(string eventId) =>
            new ArgumentException($"No event exists with the Id {eventId}.", nameof(eventId));
    }
}
