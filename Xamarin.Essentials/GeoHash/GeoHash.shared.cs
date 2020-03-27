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
            var bounds = Bounds(geohash); // <-- the hard work

            // now just determine the centre of the cell...

            double latMin = bounds.SW.Latitude, lonMin = bounds.SW.Longitude;
            double latMax = bounds.NE.Latitude, lonMax = bounds.NE.Longitude;

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
        public static Bound Bounds(string geohash)
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

            return new Bound(new GeoPoint(latMin, lonMin), new GeoPoint(latMax, lonMax));
        }

        /// <summary>
        /// Get the adjacent geohash
        /// </summary>
        /// <param name="geohash">main geohash</param>
        /// <param name="direction">direction of geohash</param>
        /// <returns>geohash of Neighbouring cell</returns>
        public static string Adjacent(string geohash, Direction direction)
        {
            geohash = geohash.ToLower();

            if (geohash.Length == 0)
                throw new Exception("Invalid geohash");

            var neighbour = new Dictionary<Direction, string[]>()
            {
                {
                    Direction.North, new string[]
                    {
                        "p0r21436x8zb9dcf5h7kjnmqesgutwvy",
                        "bc01fg45238967deuvhjyznpkmstqrwx"
                    }
                },
                {
                    Direction.South, new string[]
                    {
                        "14365h7k9dcfesgujnmqp0r2twvyx8zb",
                        "238967debc01fg45kmstqrwxuvhjyznp"
                    }
                },
                {
                     Direction.East, new string[]
                     {
                         "bc01fg45238967deuvhjyznpkmstqrwx",
                         "p0r21436x8zb9dcf5h7kjnmqesgutwvy"
                     }
                },
                {
                     Direction.West, new string[]
                     {
                         "238967debc01fg45kmstqrwxuvhjyznp",
                         "14365h7k9dcfesgujnmqp0r2twvyx8zb"
                     }
                }
            };
            var border = new Dictionary<Direction, string[]>()
            {
                {
                    Direction.North, new string[]
                    {
                        "prxz", "bcfguvyz"
                    }
                },
                {
                    Direction.South, new string[]
                    {
                        "028b", "0145hjnp"
                    }
                },
                {
                     Direction.East, new string[]
                     {
                         "bcfguvyz", "prxz"
                     }
                },
                {
                     Direction.West, new string[]
                     {
                         "0145hjnp", "028b"
                     }
                }
            };

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
            var n = Adjacent(geohash, Direction.North);
            var ne = Adjacent(Adjacent(geohash, Direction.North), Direction.East);
            var e = Adjacent(geohash, Direction.East);
            var se = Adjacent(Adjacent(geohash, Direction.South), Direction.East);
            var s = Adjacent(geohash, Direction.South);
            var sw = Adjacent(Adjacent(geohash, Direction.South), Direction.West);
            var w = Adjacent(geohash, Direction.West);
            var nw = Adjacent(Adjacent(geohash, Direction.North), Direction.West);

            return new Neighbour(n, s, e, w, ne, se, nw, sw);
        }

        /// <summary>
        /// Get hash table containg the cell-sizes with precision as key from 1-12
        /// </summary>
        public static Dictionary<int, CellSize> BoxSizeTable { get; } = new Dictionary<int, CellSize>()
        {
                   { 1, new CellSize(5000000, 5000000) },
                   { 2, new CellSize(1250000, 625000) },
                   { 3, new CellSize(156000, 156000) },
                   { 4, new CellSize(39100, 19500) },
                   { 5, new CellSize(39100, 19500) },
                   { 6, new CellSize(1220, 610) },
                   { 7, new CellSize(153, 153) },
                   { 8, new CellSize(38.2, 19.1) },
                   { 9, new CellSize(4.77, 4.77) },
                   { 10, new CellSize(1.19, 0.596) },
                   { 11, new CellSize(0.149, 0.149) },
                   { 12,  new CellSize(0.0372, 0.0186) }
        };
    }

    public struct CellSize
    {
        /// <summary>
        /// Width of the GeoCell
        /// </summary>
        public double Width { get; }

        /// <summary>
        /// Height of the GeoCell
        /// </summary>
        public double Height { get; }

        /// <summary>
        /// Pathagorean Diagonal of the GeoCell
        /// </summary>
        public double Diagonal => Math.Sqrt((Width * Width) + (Height * Height));

        public CellSize(double w, double h)
        {
            Width = w;
            Height = h;
        }
    }

    public struct GeoPoint
    {
        public double Latitude { get; }

        public double Longitude { get; }

        public GeoPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }

    public struct Bound
    {
        public GeoPoint SW { get; }

        public GeoPoint NE { get; }

        public Bound(GeoPoint sw, GeoPoint ne)
        {
            SW = sw;
            NE = ne;
        }
    }

    public enum Direction
    {
        East,
        West,
        North,
        South
    }

    public struct Neighbour
    {
        public string North { get; }

        public string South { get; }

        public string East { get; }

        public string West { get; }

        public string NorthEast { get; }

        public string SouthEast { get; }

        public string NorthWest { get; }

        public string SouthWest { get; }

        public Neighbour(string n, string s, string e, string w, string ne, string se, string nw, string sw)
        {
            North = n;
            South = s;
            East = e;
            West = w;
            NorthEast = ne;
            SouthEast = se;
            NorthWest = nw;
            SouthWest = sw;
        }
    }
}
