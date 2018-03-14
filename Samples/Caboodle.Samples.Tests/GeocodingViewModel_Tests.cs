using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caboodle.Samples.ViewModel;
using Microsoft.Caboodle;
using Moq;
using Xunit;

namespace Caboodle.Samples.Tests
{
    public class GeocodingViewModel_Tests
    {
        [Fact]
        public void Get_Address()
        {
            Geocoding.Current = Mock.Of<IGeocoding>();

            Mock.Get(Geocoding.Current)
                .Setup(x => x.GetPlacemarksAsync(10, 20))
                .ReturnsAsync(new Placemark[] { new Placemark { FeatureName = "Test" } });

            var viewModel = new GeocodingViewModel();

            viewModel.Latitude = "10";
            viewModel.Longitude = "20";

            viewModel.GetAddressCommand.Execute(null);

            Assert.NotNull(viewModel.GeocodeAddress);
            Assert.Contains("FeatureName: Test", viewModel.GeocodeAddress);
        }
    }
}
