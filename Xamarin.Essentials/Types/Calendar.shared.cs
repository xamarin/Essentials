using System;
using System.Collections.Generic;

namespace Xamarin.Essentials
{
    public class Calendar
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class CalendarEvent
    {
        public string Id { get; set; }

        public string CalendarId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public bool AllDay { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public TimeSpan Duration =>
            AllDay ? TimeSpan.FromDays(1) : EndDate - StartDate;

        public IEnumerable<CalendarEventAttendee> Attendees { get; set; }
    }

    public class CalendarEventAttendee
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }
}
