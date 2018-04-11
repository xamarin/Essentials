using System;
using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Essentials
{
    public static partial class Connectivity
    {
        private static event ConnectivityChangedEventHandler ConnectivityChanagedInternal;

        private static NetworkAccess currentAccess;

        private static List<ConnectionProfile> currentProfiles;

        public static event ConnectivityChangedEventHandler ConnectivityChanged
        {
            add
            {
                var wasRunning = ConnectivityChanagedInternal != null;

                ConnectivityChanagedInternal += value;

                if (!wasRunning && ConnectivityChanagedInternal != null)
                {
                    SetCurrent();
                    StartListeners();
                }
            }

            remove
            {
                var wasRunning = ConnectivityChanagedInternal != null;

                ConnectivityChanagedInternal -= value;

                if (wasRunning && ConnectivityChanagedInternal == null)
                    StopListeners();
            }
        }

        private static void SetCurrent()
        {
            currentAccess = NetworkAccess;
            currentProfiles = new List<ConnectionProfile>(Profiles);
        }

        private static void OnConnectivityChanged(NetworkAccess access, IEnumerable<ConnectionProfile> profiles)
            => OnConnectivityChanged(new ConnectivityChangedEventArgs(access, profiles));

        private static void OnConnectivityChanged()
            => OnConnectivityChanged(NetworkAccess, Profiles);

        private static void OnConnectivityChanged(ConnectivityChangedEventArgs e)
        {
            if (currentAccess != e.NetworkAccess ||
                !currentProfiles.SequenceEqual(e.Profiles))
            {
                SetCurrent();
                Platform.BeginInvokeOnMainThread(() => ConnectivityChanagedInternal?.Invoke(e));
            }
        }
    }

    public delegate void ConnectivityChangedEventHandler(ConnectivityChangedEventArgs e);

    public class ConnectivityChangedEventArgs : EventArgs
    {
        internal ConnectivityChangedEventArgs(NetworkAccess access, IEnumerable<ConnectionProfile> profiles)
        {
            NetworkAccess = access;
            Profiles = profiles;
        }

        public NetworkAccess NetworkAccess { get; }

        public IEnumerable<ConnectionProfile> Profiles { get; }
    }
}
