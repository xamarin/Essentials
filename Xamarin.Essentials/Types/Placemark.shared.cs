using System;
using System.Text;

namespace Xamarin.Essentials
{
    public class Placemark
    {
        public Placemark()
        {
        }

        public Placemark(Placemark placemark)
        {
            if (placemark == null)
                throw new ArgumentNullException(nameof(placemark));

            if (placemark.Location == null)
                Location = new Location();
            else
                Location = new Location(placemark.Location);

            CountryCode = placemark.CountryCode;
            CountryName = placemark.CountryName;
            FeatureName = placemark.FeatureName;
            PostalCode = placemark.PostalCode;
            Locality = placemark.Locality;
            SubLocality = placemark.SubLocality;
            Thoroughfare = placemark.Thoroughfare;
            SubThoroughfare = placemark.SubThoroughfare;
            SubAdminArea = placemark.SubAdminArea;
            AdminArea = placemark.AdminArea;
        }

        public Location? Location { get; set; }

        public string? CountryCode { get; set; }

        public string? CountryName { get; set; }

        public string? FeatureName { get; set; }

        public string? PostalCode { get; set; }

        public string? SubLocality { get; set; }

        public string? Thoroughfare { get; set; }

        public string? SubThoroughfare { get; set; }

        public string? Locality { get; set; }

        public string? AdminArea { get; set; }

        public string? SubAdminArea { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            void AppendProperty(string propertyname, string? value, bool isLast = false)
            {
                sb.Append(propertyname);
                sb.Append(": ");
                sb.Append(value);
                if (!isLast)
                    sb.Append(", ");
            }
            AppendProperty(nameof(Location), Location?.ToString());
            AppendProperty(nameof(CountryCode), CountryCode);
            AppendProperty(nameof(CountryName), CountryName);
            AppendProperty(nameof(FeatureName), FeatureName);
            AppendProperty(nameof(PostalCode), PostalCode);
            AppendProperty(nameof(SubLocality), SubLocality);
            AppendProperty(nameof(Thoroughfare), Thoroughfare);
            AppendProperty(nameof(SubThoroughfare), SubThoroughfare);
            AppendProperty(nameof(Locality), Locality);
            AppendProperty(nameof(AdminArea), AdminArea);
            AppendProperty(nameof(SubAdminArea), SubAdminArea);
            AppendProperty(nameof(Locality), Locality);
            AppendProperty(nameof(Locality), Locality, true);
            return sb.ToString();
        }
    }
}
