using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Essentials
{
    public class ListeningRequest
    {
        public ListeningRequest()
        {
            MinimumTime = TimeSpan.FromSeconds(1);
            DesiredAccuracy = GeolocationAccuracy.Default;
        }

        public ListeningRequest(GeolocationAccuracy accuracy)
        {
            MinimumTime = TimeSpan.FromSeconds(1);
            DesiredAccuracy = accuracy;
        }

        public ListeningRequest(GeolocationAccuracy accuracy, TimeSpan timeout)
        {
            MinimumTime = timeout;
            DesiredAccuracy = accuracy;
        }

        public TimeSpan MinimumTime { get; set; }

        public GeolocationAccuracy DesiredAccuracy { get; set; }

        public override string ToString() =>
            $"{nameof(DesiredAccuracy)}: {DesiredAccuracy}, {nameof(MinimumTime)}: {MinimumTime}";
    }
}
