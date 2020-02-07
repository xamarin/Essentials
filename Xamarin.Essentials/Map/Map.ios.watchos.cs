using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Contacts;
using CoreLocation;
using Foundation;
using MapKit;

namespace Xamarin.Essentials
{
    public static partial class Map
    {
        internal static Task PlatformOpenMapsAsync(double latitude, double longitude, MapLaunchOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.Name))
                options.Name = string.Empty;

            NSDictionary dictionary = null;
            var placemark = new MKPlacemark(new CLLocationCoordinate2D(latitude, longitude), dictionary);
            return OpenPlacemark(placemark, options);
        }

        internal static async Task PlatformOpenMapsAsync(Placemark placemark, MapLaunchOptions options)
        {
            var address = $"{placemark.Thoroughfare} {placemark.Locality} {placemark.AdminArea} {placemark.PostalCode} {placemark.CountryName}";

            var coder = new CLGeocoder();
            CLPlacemark[] placemarks;
            try
            {
                placemarks = await coder.GeocodeAddressAsync(address);
            }
            catch
            {
                Debug.WriteLine("Unable to get geocode address from address");
                return;
            }

            if ((placemarks?.Length ?? 0) == 0)
            {
                Debug.WriteLine("No locations exist, please check address.");
                return;
            }

            await OpenPlacemark(
                new MKPlacemark(placemarks[0].Location.Coordinate, new CNMutablePostalAddress
            {
                City = placemark.Locality ?? string.Empty,
                Country = placemark.CountryName ?? string.Empty,
                State = placemark.AdminArea ?? string.Empty,
                Street = placemark.Thoroughfare ?? string.Empty,
                PostalCode = placemark.PostalCode ?? string.Empty,
                IsoCountryCode = placemark.CountryCode ?? string.Empty
            }), options);
        }

        static Task OpenPlacemark(MKPlacemark placemark, MapLaunchOptions options)
        {
            var mapItem = new MKMapItem(placemark)
            {
                Name = options.Name ?? string.Empty
            };

            MKLaunchOptions launchOptions = null;
            if (options.NavigationMode != NavigationMode.None)
            {
                var mode = MKDirectionsMode.Default;

                switch (options.NavigationMode)
                {
                    case NavigationMode.Driving:
                        mode = MKDirectionsMode.Driving;
                        break;
                    case NavigationMode.Transit:
                        mode = MKDirectionsMode.Transit;
                        break;
                    case NavigationMode.Walking:
                        mode = MKDirectionsMode.Walking;
                        break;
                    case NavigationMode.Default:
                        mode = MKDirectionsMode.Default;
                        break;
                }
                launchOptions = new MKLaunchOptions
                {
                    DirectionsMode = mode
                };
            }

            var mapItems = new[] { mapItem };
            MKMapItem.OpenMaps(mapItems, launchOptions);
            return Task.CompletedTask;
        }
    }
}
