using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;

namespace Xamarin.Essentials
{
    public static partial class Calendar
    {
        static async Task<IEnumerable<DeviceCalendar>> PlatformGetCalendarsAsync()
        {
            await Permissions.RequireAsync(PermissionType.CalendarRead);

            var instance = await CalendarRequest.GetInstanceAsync();
            var uwpCalendarList = await instance.FindAppointmentCalendarsAsync(FindAppointmentCalendarsOptions.IncludeHidden);

            var calendars = (from calendar in uwpCalendarList
                                select new DeviceCalendar
                                {
                                    Id = calendar.LocalId,
                                    Name = calendar.DisplayName
                                }).ToList();

            return calendars;
        }

        static async Task<IEnumerable<DeviceEvent>> PlatformGetEventsAsync(string calendarId = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {
            await Permissions.RequireAsync(PermissionType.CalendarRead);

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
                             select new DeviceEvent
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

            return eventList;
        }

        static async Task<DeviceEvent> PlatformGetEventByIdAsync(string eventId)
        {
            await Permissions.RequireAsync(PermissionType.CalendarRead);

            var instance = await CalendarRequest.GetInstanceAsync();

            Appointment e;
            try
            {
                e = await instance.GetAppointmentAsync(eventId);
                e.DetailsKind = AppointmentDetailsKind.PlainText;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"[UWP]: No Event found for event Id {eventId}");
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException($"[UWP]: No Event found for event Id {eventId}");
            }

            return new DeviceEvent()
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

        static IEnumerable<DeviceEventAttendee> GetAttendeesForEvent(IEnumerable<AppointmentInvitee> inviteList)
        {
            var attendees = (from attendee in inviteList
                             select new DeviceEventAttendee
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
