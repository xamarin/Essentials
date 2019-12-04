using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Essentials
{
    // iOS
    public class DeviceSpecificAttendeeDetails
    {
        public AttendanceStatus Status { get; set; }

        public Roles Role { get; set; }
    }

    public enum AttendanceStatus
    {
        Unknown,
        Pending,
        Accepted,
        Declined,
        Tentative,
        Delegated,
        Completed,
        InProcess
    }

    public enum Roles
    {
        Unknown,
        Required,
        Optional,
        Chair,
        NonParticipant
    }
}
