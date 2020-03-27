using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xunit;

namespace Tests
{
    public class GeoHash_Test
    {
        [Fact]
        public void EncodeFromCoords()
        {
            var actual8 = "eusftqx9";
            string calculated8 = GeoHash.Encode(25.78792, -4.32913, 8);
            Assert.Equal(actual8, calculated8);
            Assert.Equal("efkbt6rx", GeoHash.Encode(new GeoPoint(12.7578, -4.32913), 8));
        }

        [Fact]
        public void DecodeFromCoords()
        {
            double lat_actual5 = 25.787, long_actual5 = -4.3291;
            var calculated8 = GeoHash.Decode("eusftqx9");
            Assert.Equal(calculated8.Latitude.ToString().Substring(0, 6), lat_actual5.ToString());
            Assert.Equal(calculated8.Longitude.ToString().Substring(0, 7), long_actual5.ToString());
        }

        [Fact]
        public void DirectionEncodeTest()
        {

            Assert.Equal("eusftqxd", GeoHash.Adjacent("eusftqx9", Direction.North));
            Assert.Equal("eusftqx8", GeoHash.Adjacent("eusftqx9", Direction.South));
            Assert.Equal("eusftqxc", GeoHash.Adjacent("eusftqx9", Direction.East));
            Assert.Equal("eusftqx3", GeoHash.Adjacent("eusftqx9", Direction.West));
        }

        [Fact]
        public void NeiboursTest()
        {
            var bounds = GeoHash.Neighbours("efkbt6rx");
            Assert.Equal("efkbt6x8", bounds.North);
            Assert.Equal("efkbt6rw", bounds.South);
            Assert.Equal("efkbt6rr", bounds.West);
            Assert.Equal("efkbt6rz", bounds.East);
            Assert.Equal("efkbt6ry", bounds.SouthEast);
            Assert.Equal("efkbt6xb", bounds.NorthEast);
            Assert.Equal("efkbt6rq", bounds.SouthWest);
            Assert.Equal("efkbt6x2", bounds.NorthWest);
        }
    }
}
