using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class CalendarViewModel : BaseViewModel
    {
        const int endDateDaysToOffset = 14;

        const string endOfDay = "23:59";

        public CalendarViewModel()
        {
            GetCalendars = new Command(OnClickGetCalendars);
            StartDateSelectedCommand = new Command(OnStartDateSelected);
            StartTimeSelectedCommand = new Command(OnStartTimeSelected);
            EndDateSelectedCommand = new Command(OnEndDateSelected);
            EndTimeSelectedCommand = new Command(OnEndTimeSelected);
            StartDateEnabledCheckBoxChanged = new Command(OnStartCheckboxChanged);
            EndDateEnabledCheckBoxChanged = new Command(OnEndCheckboxChanged);
        }

        DeviceCalendar selectedCalendar;

        bool startdatePickersEnabled;

        bool enddatePickersEnabled;

        public bool StartDatePickersEnabled
        {
            get => startdatePickersEnabled;
            set => SetProperty(ref startdatePickersEnabled, value);
        }

        public bool EndDatePickersEnabled
        {
            get => enddatePickersEnabled;
            set => SetProperty(ref enddatePickersEnabled, value);
        }

        public ICommand GetCalendars { get; }

        public ICommand StartDateEnabledCheckBoxChanged { get; }

        public ICommand EndDateEnabledCheckBoxChanged { get; }

        public ICommand StartDateSelectedCommand { get; }

        public ICommand StartTimeSelectedCommand { get; }

        public ICommand EndDateSelectedCommand { get; }

        public ICommand EndTimeSelectedCommand { get; }

        public DateTime StartDate { get; set; } = DateTime.Now;

        public TimeSpan StartTime { get; set; }

        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(endDateDaysToOffset);

        public TimeSpan EndTime { get; set; } = TimeSpan.Parse(endOfDay);

        public bool HasCalendarReadAccess { get; set; }

        public ObservableCollection<DeviceCalendar> Calendars { get; } = new ObservableCollection<DeviceCalendar>();

        public ObservableCollection<DeviceEvent> Events { get; } = new ObservableCollection<DeviceEvent>();

        public DeviceCalendar SelectedCalendar
        {
            get => selectedCalendar;

            set
            {
                if (SetProperty(ref selectedCalendar, value) && selectedCalendar != null)
                {
                    OnChangeRequestCalendarSpecificEvents(selectedCalendar.Id);
                }
            }
        }

        void OnStartCheckboxChanged(object parameter)
        {
            if (parameter is bool b)
            {
                StartDatePickersEnabled = b;

                RefreshEventList(SelectedCalendar?.Id);
            }
        }

        void OnEndCheckboxChanged(object parameter)
        {
            if (parameter is bool b)
            {
                EndDatePickersEnabled = b;

                RefreshEventList(SelectedCalendar?.Id);
            }
        }

        async void OnClickGetCalendars()
        {
            Calendars.Clear();
            Calendars.Add(new DeviceCalendar() { Id = null, Name = "All" });
            var calendars = await Calendar.GetCalendarsAsync();
            foreach (var calendar in calendars)
            {
                Calendars.Add(calendar);
            }
            SelectedCalendar = Calendars[0];
        }

        void OnStartDateSelected(object parameter)
        {
            var startDate = parameter as DateTime?;

            if (!startDate.HasValue)
                return;

            startDate += StartTime;

            RefreshEventList(SelectedCalendar?.Id, startDate);
        }

        void OnStartTimeSelected(object parameter)
        {
            if (parameter == null)
                return;

            RefreshEventList(SelectedCalendar?.Id);
        }

        void OnEndDateSelected(object parameter)
        {
            var endDate = parameter as DateTime?;

            if (!endDate.HasValue)
                return;

            endDate += EndTime;
            RefreshEventList(SelectedCalendar?.Id, null, endDate);
        }

        void OnEndTimeSelected(object parameter)
        {
            if (parameter == null)
                return;

            RefreshEventList();
        }

        void OnChangeRequestCalendarSpecificEvents(string calendarId = null, DateTime? startDateTime = null, DateTime? endDateTime = null) => RefreshEventList(calendarId, startDateTime, endDateTime);

        async void RefreshEventList(string calendarId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            startDate = StartDatePickersEnabled && !startDate.HasValue ? (DateTime?)StartDate.Date + StartTime : startDate;
            endDate = (EndDatePickersEnabled && !endDate.HasValue) ? (DateTime?)EndDate.Date + EndTime : endDate;
            if (Calendars.Count == 0)
                return;

            Events.Clear();
            var events = await Calendar.GetEventsAsync(calendarId, startDate, endDate);
            foreach (var calendarEvent in events)
            {
                Events.Add(calendarEvent);
            }
        }
    }
}
