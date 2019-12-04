using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Appointments;

namespace Xamarin.Essentials
{
    static class CalendarRequest
    {
        static AppointmentStore uwpAppointmentStore;

        public static async System.Threading.Tasks.Task<AppointmentStore> GetInstanceAsync(AppointmentStoreAccessType type = AppointmentStoreAccessType.AllCalendarsReadOnly)
        {
            if (uwpAppointmentStore == null)
            {
                uwpAppointmentStore = await AppointmentManager.RequestStoreAsync(type);
            }

            return uwpAppointmentStore;
        }
    }
}
