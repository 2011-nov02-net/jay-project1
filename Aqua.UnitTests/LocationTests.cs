using Aqua.Data;
using Aqua.Library;
using Aqua.WebApp.Controllers;
using Aqua.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Aqua.UnitTests
{
    public class LocationTests
    {
        [Fact]
        public void Index_Get_Locations()
        {
            // Arrange
            Mock<ILocationRepo> mockRepo = new Mock<ILocationRepo>();
            List<Location> locationList = new List<Location>();
            Location cali = new Location
            {
                Id = 1,
                City = "cali"
            };
            locationList.Add(cali);
            mockRepo.Setup(r => r.GetAllLocations())
                .Returns
                    (locationList);
            LocationController controller = new LocationController(new NullLogger<LocationController>(), mockRepo.Object, null, null);
            // Act
            IActionResult actionResult = controller.Index();
            // Assert
            ViewResult viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            List<LocationViewModel> locations = Assert.IsAssignableFrom<IEnumerable<LocationViewModel>>(viewResult.Model).ToList();
            Assert.Equal("cali", locations[0].City);
            Assert.Equal(1, locations[0].Id);
        }
        [Fact]
        public void Index_Get_Multiple_Locations()
        {
            // Arrange
            Mock<ILocationRepo> mockRepo = new Mock<ILocationRepo>();
            List<Location> locationList = new List<Location>();
            Location cali = new Location
            {
                Id = 1,
                City = "cali"
            };
            Location hk = new Location
            {
                Id = 2,
                City = "hongkong"
            };
            locationList.Add(cali);
            locationList.Add(hk);
            mockRepo.Setup(r => r.GetAllLocations())
                .Returns
                    (locationList);
            LocationController controller = new LocationController(new NullLogger<LocationController>(), mockRepo.Object, null, null);
            // Act
            IActionResult actionResult = controller.Index();
            // Assert
            ViewResult viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            List<LocationViewModel> locations = Assert.IsAssignableFrom<IEnumerable<LocationViewModel>>(viewResult.Model).ToList();
            Assert.Equal(2, locations.Count()); // Number of objects in location list
            Assert.Equal(2, locations[1].Id); // Return Id of the second location (hk)
        }
        [Fact]
        public void Index_Get_Valid_Location()
        {
            // Arrange
            Mock<ILocationRepo> mockRepo = new Mock<ILocationRepo>();
            List<Location> locationList = new List<Location>();
            Location cali = new Location
            {
                Id = 1
            };
            locationList.Add(cali);
            mockRepo.Setup(r => r.GetAllLocations())
                .Returns
                    (locationList);
            LocationController controller = new LocationController(new NullLogger<LocationController>(), mockRepo.Object, null, null);
            // Act
            IActionResult actionResult = controller.Index();
            // Assert
            Assert.True(controller.ModelState.IsValid);
        }
        [Fact]
        public void Create_Valid_Location()
        {
            // Arrange
            Mock<ILocationRepo> mockRepo = new Mock<ILocationRepo>();
            Location hk = new Location
            {
                Id = 1,
                City = "hongkong"
            };
            LocationController controller = new LocationController(new NullLogger<LocationController>(), mockRepo.Object, null, null);
            // Act
            controller.Create(hk);
            // Assert
            Assert.True(controller.ModelState.IsValid);
        }
    }
}
