using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Essentials
{
    // Android
    public class DeviceSpecificAttendeeDetails
    {
        public bool IsAttending { get; set; }

        public AttendanceStatus Status { get; set; }
    }

    public enum AttendanceStatus
    {
        None,
        Accepted,
        Declined,
        Invited,
        Tentative
    }
}
