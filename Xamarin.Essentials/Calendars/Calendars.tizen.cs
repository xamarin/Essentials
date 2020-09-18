using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCalendar = Tizen.Pims.Calendar;
using TCalendarViews = Tizen.Pims.Calendar.CalendarViews;

namespace Xamarin.Essentials
{
    public static partial class Calendars
    {
        static TCalendar.CalendarManager manager = new TCalendar.CalendarManager();

        static Task<IEnumerable<Calendar>> PlatformGetCalendarsAsync()
        {
            var calendarList = manager.Database.GetAll(TCalendarViews.Book.Uri, 0, 0);
            return Task.FromResult<IEnumerable<Calendar>>(ToCalendars(calendarList).ToList());
        }

        static IEnumerable<Calendar> ToCalendars(TCalendar.CalendarList calendarList)
        {
            calendarList.MoveFirst();
            for (var i = 0; i < calendarList.Count; i++)
            {
                yield return ToCalendar(calendarList.GetCurrentRecord());
                calendarList.MoveNext();
            }
        }

        static Calendar ToCalendar(TCalendar.CalendarRecord calendarRecord)
        {
            return new Calendar
            {
                Id = calendarRecord.Get<int>(TCalendarViews.Book.Id).ToString(),
                Name = calendarRecord.Get<string>(TCalendarViews.Book.Name),
            };
        }

        static Task<Calendar> PlatformGetCalendarAsync(string calendarId)
        {
            if (!string.IsNullOrEmpty(calendarId) && !int.TryParse(calendarId, out _))
                throw InvalidCalendar(calendarId);

            var bookUri = TCalendarViews.Book.Uri;
            var bookId = TCalendarViews.Book.Id;
            var query = new TCalendar.CalendarQuery(bookUri);
            var filter = new TCalendar.CalendarFilter(bookUri, bookId, TCalendar.CalendarFilter.IntegerMatchType.Equal, int.Parse(calendarId));
            query.SetFilter(filter);

            var calendarList = manager.Database.GetRecordsWithQuery(query, 0, 0);
            if (calendarList.Count <= 0)
                throw InvalidCalendar(calendarId);
            return Task.FromResult(ToCalendar(calendarList.GetCurrentRecord()));
        }

        static Task<IEnumerable<CalendarEvent>> PlatformGetEventsAsync(string calendarId, DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            if (!string.IsNullOrEmpty(calendarId) && !int.TryParse(calendarId, out _))
                throw InvalidCalendar(calendarId);

            var sDate = startDate ?? DateTimeOffset.Now.Add(defaultStartTimeFromNow);
            var eDate = endDate ?? sDate.Add(defaultEndTimeFromStartTime);
            var sTime = new TCalendar.CalendarTime(sDate.UtcTicks);
            var eTime = new TCalendar.CalendarTime(eDate.UtcTicks);

            var eventUri = TCalendarViews.Event.Uri;
            var query = new TCalendar.CalendarQuery(eventUri);
            var filter = new TCalendar.CalendarFilter(
                eventUri,
                TCalendarViews.Event.Start,
                TCalendar.CalendarFilter.IntegerMatchType.GreaterThanOrEqual,
                sTime);
            filter.AddCondition(
                TCalendar.CalendarFilter.LogicalOperator.And,
                TCalendarViews.Event.End,
                TCalendar.CalendarFilter.IntegerMatchType.LessThanOrEqual,
                eTime);
            if (!string.IsNullOrEmpty(calendarId))
            {
                filter.AddCondition(
                    TCalendar.CalendarFilter.LogicalOperator.And,
                    TCalendarViews.Event.BookId,
                    TCalendar.CalendarFilter.IntegerMatchType.Equal,
                    int.Parse(calendarId));
            }
            query.SetFilter(filter);
            query.SetSort(TCalendarViews.Event.Start, true);

            var eventList = manager.Database.GetRecordsWithQuery(query, 0, 0);

            return Task.FromResult<IEnumerable<CalendarEvent>>(ToEvents(eventList).ToList());
        }

        static IEnumerable<CalendarEvent> ToEvents(TCalendar.CalendarList eventList)
        {
            eventList.MoveFirst();
            for (var i = 0; i < eventList.Count; i++)
            {
                yield return ToEvent(eventList.GetCurrentRecord());
                eventList.MoveNext();
            }
        }

        static CalendarEvent ToEvent(TCalendar.CalendarRecord eventRecord)
        {
            var sTime = eventRecord.Get<TCalendar.CalendarTime>(TCalendarViews.Event.Start).UtcTime;
            var eTime = eventRecord.Get<TCalendar.CalendarTime>(TCalendarViews.Event.End).UtcTime;
            var sDate = DateTimeOffset.FromUnixTimeMilliseconds(sTime.Millisecond);
            var eDate = DateTimeOffset.FromUnixTimeMilliseconds(eTime.Millisecond);

            return new CalendarEvent
            {
                Id = eventRecord.Get<int>(TCalendarViews.Event.Id).ToString(),
                CalendarId = eventRecord.Get<int>(TCalendarViews.Event.BookId).ToString(),
                Title = eventRecord.Get<string>(TCalendarViews.Event.Summary),
                Description = eventRecord.Get<string>(TCalendarViews.Event.Description),
                Location = eventRecord.Get<string>(TCalendarViews.Event.Location),
                AllDay = eventRecord.Get<bool>(TCalendarViews.Event.IsAllday),
                StartDate = sDate,
                EndDate = eDate,
                Attendees = PlatformGetAttendees(eventRecord.Get<string>(TCalendarViews.Event.Id)),
            };
        }

        static IEnumerable<CalendarEventAttendee> PlatformGetAttendees(string eventId)
        {
            if (!string.IsNullOrEmpty(eventId) && !int.TryParse(eventId, out _))
                throw InvalidCalendar(eventId);

            var eventUri = TCalendarViews.Event.Uri;
            var query = new TCalendar.CalendarQuery(eventUri);
            var filter = new TCalendar.CalendarFilter(TCalendarViews.Event.Uri, TCalendarViews.Event.Id, TCalendar.CalendarFilter.StringMatchType.FullString, eventId);
            query.SetFilter(filter);

            var attendeeList = manager.Database.GetRecordsWithQuery(query, 0, 0);
            if (attendeeList.Count <= 0)
                throw InvalidEvent(eventId);
            return ToAttendees(attendeeList).ToList();
        }

        static IEnumerable<CalendarEventAttendee> ToAttendees(TCalendar.CalendarList attendeeList)
        {
            attendeeList.MoveFirst();
            for (var i = 0; i < attendeeList.Count; i++)
            {
                yield return ToAttendee(attendeeList.GetCurrentRecord());
                attendeeList.MoveNext();
            }
        }

        static CalendarEventAttendee ToAttendee(TCalendar.CalendarRecord attendeeRecord)
        {
            return new CalendarEventAttendee
            {
                Name = attendeeRecord.Get<string>(TCalendarViews.Event.Summary),
                Email = attendeeRecord.Get<string>(TCalendarViews.Event.EmailId),
            };
        }

        static Task<CalendarEvent> PlatformGetEventAsync(string eventId)
        {
            if (!string.IsNullOrEmpty(eventId) && !int.TryParse(eventId, out _))
                throw InvalidCalendar(eventId);

            var eventUri = TCalendarViews.Event.Uri;
            var query = new TCalendar.CalendarQuery(eventUri);
            var filter = new TCalendar.CalendarFilter(
                eventUri,
                TCalendarViews.Event.Id,
                TCalendar.CalendarFilter.IntegerMatchType.Equal,
                int.Parse(eventId));
            query.SetFilter(filter);

            var eventList = manager.Database.GetRecordsWithQuery(query, 0, 0);
            if (eventList.Count <= 0)
                throw InvalidEvent(eventId);
            eventList.MoveFirst();
            return Task.FromResult(ToEvent(eventList.GetCurrentRecord()));
        }
    }
}
