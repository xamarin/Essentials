using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Xamarin.Essentials
{
    public static class GeoHash
    {
        internal const string base32 = "0123456789bcdefghjkmnpqrstuvwxyz"; // (geohash-specific) Base32 map

        /// <summary>
        /// Encode given latitude and longitude to corresponding GeoHash
        /// </summary>
        /// <param name="lat">latitude of the Location</param>
        /// <param name="lon">longitude of the Location</param>
        /// <param name="precision">Number of characters in resulting geohash </param>
        /// <returns>corresponding geohash</returns>
        public static string Encode(double lat, double lon, int precision)
        {
            var idx = 0; // index into base32 map
            var bit = 0; // each char holds 5 bits
            var evenBit = true;
            var geohash = string.Empty;

            double latMin = -90, latMax = 90;
            double lonMin = -180, lonMax = 180;

            while (geohash.Length < precision)
            {
                if (evenBit)
                {
                    // bisect E-W longitude
                    var lonMid = (lonMin + lonMax) / 2;
                    if (lon >= lonMid)
                    {
                        idx = (idx * 2) + 1;
                        lonMin = lonMid;
                    }
                    else
                    {
                        idx = idx * 2;
                        lonMax = lonMid;
                    }
                }
                else
                {
                    // bisect N-S latitude
                    var latMid = (latMin + latMax) / 2.0;
                    if (lat >= latMid)
                    {
                        idx = (idx * 2) + 1;
                        latMin = latMid;
                    }
                    else
                    {
                        idx = idx * 2;
                        latMax = latMid;
                    }
                }
                evenBit = !evenBit;

                if (++bit == 5)
                {
                    // 5 bits gives us a character: append it and start over
                    geohash += base32[idx];
                    bit = 0;
                    idx = 0;
                }
            }

            return geohash;
        }

        /// <summary>
        /// Encode given latitude and longitude to corresponding GeoHash
        /// </summary>
        /// <param name="point">Geo Point location </param>
        /// <param name="precision">Number of characters in resulting geohash</param>
        /// <returns>corresponding geohash</returns>
        public static string Encode(GeoPoint point, int precision)
        {
            return Encode(point.Latitude, point.Longitude, precision);
        }

        /// <summary>
        /// Get Corresponding Approximated Latitude and Longitude
        /// </summary>
        /// <param name="geohash">geohash of location</param>
        /// <returns>A GeoPoint (Corresponding Lat,Long)</returns>
        public static GeoPoint Decode(string geohash)
        {
            var bounds = GetBound(geohash); // <-- the hard work

            // now just determine the centre of the cell...

            double latMin = bounds.Southwest.Latitude, lonMin = bounds.Southwest.Longitude;
            double latMax = bounds.Northeast.Latitude, lonMax = bounds.Northeast.Longitude;

            // cell centre
            var lat = (latMin + latMax) / 2.0;
            var lon = (lonMin + lonMax) / 2.0;

            // round to close to centre without excessive precision: ⌊2-log10(Δ°)⌋ decimal places
            var precLat = (int)Math.Floor(2 - Math.Log((latMax - latMin) / 2.302585092994046));
            var precLong = (int)Math.Floor(2 - Math.Log((lonMax - lonMin) / 2.302585092994046));

            lat = double.Parse(lat.ToString($"F{precLat}", CultureInfo.InvariantCulture));
            lon = double.Parse(lon.ToString($"F{precLong}", CultureInfo.InvariantCulture));

            return new GeoPoint(lat, lon);
        }

        /// <summary>
        /// Get the bound of the given geohash
        /// </summary>
        /// <param name="geohash">hash of location</param>
        /// <returns>SW,NE location</returns>
        public static GeoHashBound GetBound(string geohash)
        {
            geohash = geohash.ToLower();
            var evenBit = true;
            double latMin = -90, latMax = 90;
            double lonMin = -180, lonMax = 180;

            for (var i = 0; i < geohash.Length; i++)
            {
                var chr = geohash[i];
                var idx = base32.IndexOf(chr);
                if (idx == -1)
                    throw new Exception("Invalid geohash");

                for (var n = 4; n >= 0; n--)
                {
                    var bitN = idx >> n & 1;
                    if (evenBit)
                    {
                        // longitude
                        var lonMid = (lonMin + lonMax) / 2.0;
                        if (bitN == 1)
                        {
                            lonMin = lonMid;
                        }
                        else
                        {
                            lonMax = lonMid;
                        }
                    }
                    else
                    {
                        // latitude
                        var latMid = (latMin + latMax) / 2.0;
                        if (bitN == 1)
                        {
                            latMin = latMid;
                        }
                        else
                        {
                            latMax = latMid;
                        }
                    }
                    evenBit = !evenBit;
                }
            }

            return new GeoHashBound(new GeoPoint(latMin, lonMin), new GeoPoint(latMax, lonMax));
        }

        static Dictionary<CardinalDirection, string[]> neighbour = new Dictionary<CardinalDirection, string[]>()
            {
                {
                    CardinalDirection.North, new string[]
                    {
                        "p0r21436x8zb9dcf5h7kjnmqesgutwvy",
                        "bc01fg45238967deuvhjyznpkmstqrwx"
                    }
                },
                {
                    CardinalDirection.South, new string[]
                    {
                        "14365h7k9dcfesgujnmqp0r2twvyx8zb",
                        "238967debc01fg45kmstqrwxuvhjyznp"
                    }
                },
                {
                     CardinalDirection.East, new string[]
                     {
                         "bc01fg45238967deuvhjyznpkmstqrwx",
                         "p0r21436x8zb9dcf5h7kjnmqesgutwvy"
                     }
                },
                {
                     CardinalDirection.West, new string[]
                     {
                         "238967debc01fg45kmstqrwxuvhjyznp",
                         "14365h7k9dcfesgujnmqp0r2twvyx8zb"
                     }
                }
            };

        static Dictionary<CardinalDirection, string[]> border = new Dictionary<CardinalDirection, string[]>()
            {
                {
                    CardinalDirection.North, new string[]
                    {
                        "prxz", "bcfguvyz"
                    }
                },
                {
                    CardinalDirection.South, new string[]
                    {
                        "028b", "0145hjnp"
                    }
                },
                {
                     CardinalDirection.East, new string[]
                     {
                         "bcfguvyz", "prxz"
                     }
                },
                {
                     CardinalDirection.West, new string[]
                     {
                         "0145hjnp", "028b"
                     }
                }
            };

        /// <summary>
        /// Get the adjacent geohash
        /// </summary>
        /// <param name="geohash">main geohash</param>
        /// <param name="direction">direction of geohash</param>
        /// <returns>geohash of Neighbouring cell</returns>
        public static string Adjacent(string geohash, CardinalDirection direction)
        {
            geohash = geohash.ToLower();

            if (geohash.Length == 0)
                throw new Exception("Invalid geohash");

            var lastCh = geohash[geohash.Length - 1];    // last character of hash
            var parent = geohash.Substring(0, geohash.Length - 1); // hash without last character

            var type = geohash.Length % 2;

            // check for edge-cases which don't share common prefix
            if (border[direction][type].IndexOf(lastCh) != -1 && parent != string.Empty)
            {
                parent = Adjacent(parent, direction);
            }

            // append letter for direction to parent
            return parent + base32[neighbour[direction][type].IndexOf(lastCh)];
        }

        /// <summary>
        /// Get all the neighbouring Geohashes [All 8]
        /// </summary>
        /// <param name="geohash">Current Geohash</param>
        /// <returns>Neigbour Object</returns>
        public static Neighbour Neighbours(string geohash)
        {
            var n = Adjacent(geohash, CardinalDirection.North);
            var ne = Adjacent(Adjacent(geohash, CardinalDirection.North), CardinalDirection.East);
            var e = Adjacent(geohash, CardinalDirection.East);
            var se = Adjacent(Adjacent(geohash, CardinalDirection.South), CardinalDirection.East);
            var s = Adjacent(geohash, CardinalDirection.South);
            var sw = Adjacent(Adjacent(geohash, CardinalDirection.South), CardinalDirection.West);
            var w = Adjacent(geohash, CardinalDirection.West);
            var nw = Adjacent(Adjacent(geohash, CardinalDirection.North), CardinalDirection.West);

            return new Neighbour(n, s, e, w, ne, se, nw, sw);
        }
    }

    public readonly struct GeoPoint : IEquatable<GeoPoint>
    {
        public double Latitude { get; }

        public double Longitude { get; }

        public GeoPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public override bool Equals(object obj) =>
                    (obj is GeoPoint data) && Equals(data);

        public bool Equals(GeoPoint other) =>
            Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);

        public override int GetHashCode() =>
            Latitude.GetHashCode() ^ Longitude.GetHashCode();

        public static bool operator ==(GeoPoint left, GeoPoint right) =>
            left.Equals(right);

        public static bool operator !=(GeoPoint left, GeoPoint right) =>
            !left.Equals(right);
    }

    public readonly struct GeoHashBound : IEquatable<GeoHashBound>
    {
        public GeoPoint Southwest { get; }

        public GeoPoint Northeast { get; }

        public GeoHashBound(GeoPoint southWest, GeoPoint northEast)
        {
            Southwest = southWest;
            Northeast = northEast;
        }

        public override bool Equals(object obj) =>
                    (obj is GeoHashBound data) && Equals(data);

        public bool Equals(GeoHashBound other) =>
            Southwest.Equals(other.Southwest) && Northeast.Equals(other.Northeast);

        public override int GetHashCode() =>
            Southwest.GetHashCode() ^ Northeast.GetHashCode();

        public static bool operator ==(GeoHashBound left, GeoHashBound right) =>
            left.Equals(right);

        public static bool operator !=(GeoHashBound left, GeoHashBound right) =>
            !left.Equals(right);
    }

    public enum CardinalDirection
    {
        East,
        West,
        North,
        South
    }

    public readonly struct Neighbour : IEquatable<Neighbour>
    {
        public string North { get; }

        public string South { get; }

        public string East { get; }

        public string West { get; }

        public string Northeast { get; }

        public string Southeast { get; }

        public string Northwest { get; }

        public string Southwest { get; }

        public Neighbour(string n, string s, string e, string w, string ne, string se, string nw, string sw)
        {
            North = n;
            South = s;
            East = e;
            West = w;
            Northeast = ne;
            Southeast = se;
            Northwest = nw;
            Southwest = sw;
        }

        public override bool Equals(object obj) =>
               (obj is Neighbour data) && Equals(data);

        public bool Equals(Neighbour other) =>
            (South ?? string.Empty).Equals(other.South) && (North ?? string.Empty).Equals(other.North)
            && (East ?? string.Empty).Equals(other.East) && (West ?? string.Empty).Equals(other.West);

        public override int GetHashCode() =>
            (West ?? string.Empty).GetHashCode() + (East ?? string.Empty).GetHashCode()
            + (North ?? string.Empty).GetHashCode() + (South ?? string.Empty).GetHashCode();

        public static bool operator ==(Neighbour left, Neighbour right) =>
            left.Equals(right);

        public static bool operator !=(Neighbour left, Neighbour right) =>
            !left.Equals(right);
    }
}
