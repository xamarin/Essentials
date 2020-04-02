/*
 * Verify from here http://www.movable-type.co.uk/scripts/geohash.html
 */
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xunit;

namespace Tests
{
    public class GeoHash_Tests
    {
        [Fact]
        public void EncodeFromCoords()
        {
            var actual = "eusftqx9";
            var calculated = GeoHash.Encode(25.78792, -4.32913, 8);
            Assert.Equal(actual, calculated);
            Assert.Equal("efkbt6rx", GeoHash.Encode(new GeoPoint(12.7578, -4.32913), 8));
        }

        [Fact]
        public void DecodeFromCoords()
        {
            double lat_actual = 25.787, long_actual = -4.3291;
            var calculated = GeoHash.Decode("eusftqx9");
            Assert.Equal(calculated.Latitude.ToString().Substring(0, 6), lat_actual.ToString());
            Assert.Equal(calculated.Longitude.ToString().Substring(0, 7), long_actual.ToString());
        }

        [Fact]
        public void DirectionEncodeTest()
        {
            Assert.Equal("eusftqxd", GeoHash.GetAdjacent("eusftqx9", CardinalDirection.North));
            Assert.Equal("eusftqx8", GeoHash.GetAdjacent("eusftqx9", CardinalDirection.South));
            Assert.Equal("eusftqxc", GeoHash.GetAdjacent("eusftqx9", CardinalDirection.East));
            Assert.Equal("eusftqx3", GeoHash.GetAdjacent("eusftqx9", CardinalDirection.West));
        }

        [Fact]
        public void NeiboursTest()
        {
            var bounds = GeoHash.GetNeighbours("efkbt6rx");
            Assert.Equal("efkbt6x8", bounds.North);
            Assert.Equal("efkbt6rw", bounds.South);
            Assert.Equal("efkbt6rr", bounds.West);
            Assert.Equal("efkbt6rz", bounds.East);
            Assert.Equal("efkbt6ry", bounds.Southeast);
            Assert.Equal("efkbt6xb", bounds.Northeast);
            Assert.Equal("efkbt6rq", bounds.Southwest);
            Assert.Equal("efkbt6x2", bounds.Northwest);
        }
    }
}
