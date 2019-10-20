namespace Xamarin.Essentials
{
    public enum PermissionStatus
    {
        // Denied by user
        Denied,

        // Feature is disabled on device
        Disabled,

        // Granted by user
        Granted,

        // Restricted (only iOS)
        Restricted,

        // Permission is in an unknown state
        Unknown
    }

    public enum PermissionType
    {
        /// <summary>
        /// The unknown permission only used for return type, never requested
        /// </summary>
        Unknown,

        /// <summary>
        /// Android: Battery Stats
        /// iOS: None
        /// UWP: None
        /// Tizen: None
        /// </summary>
        Battery,

        /// <summary>
        /// Android: Camera
        /// iOS: Photos (Camera Roll and Camera)
        /// UWP: None
        /// Tizen: Camera
        /// </summary>
        Camera,

        /// <summary>
        /// Android: Flashlight
        /// iOS: None
        /// UWP: None
        /// Tizen: LED
        /// </summary>
        Flashlight,

        /// <summary>
        /// Android: None
        /// iOS: None
        /// UWP: None
        /// Tizen: AppManager Launch
        /// </summary>
        LaunchApp,

        /// <summary>
        /// Android: Course Location, Fine Location
        /// iOS: Location When In Use
        /// UWP: Location
        /// Tizen: Location
        /// </summary>
        LocationWhenInUse,

        /// <summary>
        /// Android: None
        /// iOS: None
        /// UWP: None
        /// Tizen: Internet, MapService, Network Get
        /// </summary>
        Maps,

        /// <summary>
        /// Android: Network State
        /// iOS: None
        /// UWP: None
        /// Tizen: Internet, Network Get
        /// </summary>
        NetworkState,

        /// <summary>
        /// Android: Vibrate
        /// iOS: None
        /// UWP: None
        /// Tizen: Haptic
        /// </summary>
        Vibrate,

        /// <summary>
        /// Android: WriteExternalStorage
        /// iOS: None
        /// UWP: None
        /// Tizen: None
        /// </summary>
        [System.Obsolete("Use StorageWrite instead")]
        WriteExternalStorage,

        /// <summary>
        /// Android: WriteExternalStorage
        /// iOS: None
        /// UWP: None
        /// Tizen: None
        /// </summary>
        StorageWrite,

        /// <summary>
        /// Android: ReadExternalStorage
        /// iOS: None
        /// UWP: None
        /// Tizen: None
        /// </summary>
        StorageRead,

        /// <summary>
        /// Android: Read Calendar
        /// iOS:
        /// UWP:
        /// Tizen: Calendar Read, Calendar Read
        /// </summary>
        CalendarRead,

        /// <summary>
        /// Android: Write Calendar
        /// iOS:
        /// UWP:
        /// Tizen: Calendar Write, Calendar Write
        /// </summary>
        CalendarWrite,

        /// <summary>
        /// Android: Read Contacts
        /// iOS: 
        /// UWP: 
        /// Tizen: Contacts Read, Contacts Write
        /// </summary>
        ContactsRead,

        /// <summary>
        /// Android: Write Contacts	
        /// iOS: 
        /// UWP: 
        /// Tizen: Contacts Write, Contacts Write
        /// </summary>
        ContactsWrite,

        /// <summary>
        /// Android: Course Location, Fine Location, Background Location (Android 10+)
        /// iOS: Location Always
        /// UWP: Location
        /// Tizen: Location
        /// </summary>
        LocationAlways,

        /// <summary>
        /// Android: Record Audio
        /// iOS: Microphone
        /// UWP: Microphone
        /// Tizen: Recorder
        /// </summary>
        Microphone,

        /// <summary>
        /// Android: Read Phone State, Optional: Call Phone, Read Call Log, Add Voicemail, Use Sip, Process Outgoing Calls
        /// iOS: None
        /// UWP: 
        /// Tizen: Call
        /// </summary>
        Phone,

        /// <summary>
        /// Android: None
        /// iOS: Photos
        /// UWP: 
        /// Tizen: 
        /// </summary>
        Photos,

        /// <summary>
        /// Android: None
        /// iOS: Reminders
        /// UWP: 
        /// Tizen: 
        /// </summary>
        Reminders,

        /// <summary>
        /// Android: Body Sensors
        /// iOS: 
        /// UWP: 
        /// Tizen: 
        /// </summary>
        Sensors,

        /// <summary>
        /// Android: Receive Sms, Optional: Send Sms, Read Sms, Receive Mms
        /// iOS: 
        /// UWP: 
        /// Tizen: 
        /// </summary>
        Sms,

        /// <summary>
        /// Android: 
        /// iOS: 
        /// UWP: 
        /// Tizen: 
        /// </summary>
        Media,

        /// <summary>
        /// Android: 
        /// iOS: 
        /// UWP: 
        /// Tizen: 
        /// </summary>
        Speech
    }
}
