using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;

namespace Xamarin.Essentials
{
    public static partial class Calendars
    {
        static async Task<IEnumerable<Calendar>> PlatformGetCalendarsAsync()
        {
            await Permissions.RequestAsync<Permissions.CalendarRead>();

            var instance = await CalendarRequest.GetInstanceAsync();
            var uwpCalendarList = await instance.FindAppointmentCalendarsAsync(FindAppointmentCalendarsOptions.IncludeHidden);

            var calendars = (from calendar in uwpCalendarList
                                select new Calendar
                                {
                                    Id = calendar.LocalId,
                                    Name = calendar.DisplayName
                                }).ToList();

            return calendars;
        }

        static async Task<IEnumerable<CalendarEvent>> PlatformGetEventsAsync(string calendarId = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {
            await Permissions.RequestAsync<Permissions.CalendarRead>();

            var options = new FindAppointmentsOptions();
            options.FetchProperties.Add(AppointmentProperties.Subject);
            options.FetchProperties.Add(AppointmentProperties.StartTime);
            options.FetchProperties.Add(AppointmentProperties.Duration);
            options.FetchProperties.Add(AppointmentProperties.AllDay);
            var sDate = startDate ?? DateTimeOffset.Now.Add(defaultStartTimeFromNow);
            var eDate = endDate ?? sDate.Add(defaultEndTimeFromStartTime);

            if (eDate < sDate)
                eDate = sDate;

            var instance = await CalendarRequest.GetInstanceAsync();
            var events = await instance.FindAppointmentsAsync(sDate, eDate.Subtract(sDate), options);

            var eventList = (from e in events
                             select new CalendarEvent
                             {
                                 Id = e.LocalId,
                                 CalendarId = e.CalendarId,
                                 Title = e.Subject,
                                 StartDate = e.StartTime,
                                 EndDate = !e.AllDay ? (DateTimeOffset?)e.StartTime.Add(e.Duration) : null
                             })
                            .Where(e => e.CalendarId == calendarId || calendarId == null)
                            .OrderBy(e => e.StartDate)
                            .ToList();

            if (eventList.Count == 0 && !string.IsNullOrWhiteSpace(calendarId))
            {
                await GetCalendarById(calendarId);
            }

            return eventList;
        }

        static async Task<Calendar> GetCalendarById(string calendarId)
        {
            var instance = await CalendarRequest.GetInstanceAsync();
            var uwpCalendarList = await instance.FindAppointmentCalendarsAsync(FindAppointmentCalendarsOptions.IncludeHidden);

            var result = (from calendar in uwpCalendarList
                             select new Calendar
                             {
                                 Id = calendar.LocalId,
                                 Name = calendar.DisplayName
                             })
                             .Where(c => c.Id == calendarId).FirstOrDefault();
            if (result == null)
            {
                throw new ArgumentOutOfRangeException($"[UWP]: No calendar exists with the Id {calendarId}");
            }

            return result;
        }

        static async Task<CalendarEvent> PlatformGetEventByIdAsync(string eventId)
        {
            await Permissions.RequestAsync<Permissions.CalendarRead>();

            var instance = await CalendarRequest.GetInstanceAsync();

            Appointment e;
            try
            {
                e = await instance.GetAppointmentAsync(eventId);
                e.DetailsKind = AppointmentDetailsKind.PlainText;
            }
            catch (ArgumentException)
            {
                if (string.IsNullOrWhiteSpace(eventId))
                {
                    throw new ArgumentException($"[UWP]: No Event found for event Id {eventId}");
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"[UWP]: No Event found for event Id {eventId}");
                }
            }

            return new CalendarEvent()
            {
                Id = e.LocalId,
                CalendarId = e.CalendarId,
                Title = e.Subject,
                Description = e.Details,
                Location = e.Location,
                StartDate = e.StartTime,
                EndDate = !e.AllDay ? (DateTimeOffset?)e.StartTime.Add(e.Duration) : null,
                Attendees = GetAttendeesForEvent(e.Invitees)
            };
        }

        static IEnumerable<CalendarEventAttendee> GetAttendeesForEvent(IEnumerable<AppointmentInvitee> inviteList)
        {
            var attendees = (from attendee in inviteList
                             select new CalendarEventAttendee
                             {
                                 Name = attendee.DisplayName,
                                 Email = attendee.Address
                             })
                            .OrderBy(e => e.Name)
                            .ToList();

            return attendees;
        }
    }
}
