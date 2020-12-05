using System;
using Xunit;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Aqua.WebApp.Controllers;
using Aqua.WebApp.Models;
using Aqua.Data;
using Aqua.Library;
using Moq;
using System.Collections.Generic;

namespace Aqua.UnitTests
{
    public class LocationTests
    {
        [Fact]
        public void Index_Get_Locations()
        {
            // Arrange
            var mockRepo = new Mock<ILocationRepo>();
            var locationList = new List<Location>();
            var cali = new Location();
                cali.Id = 1;
                cali.City = "cali";
            locationList.Add(cali);
            mockRepo.Setup(r => r.GetAllLocations())
                .Returns
                    (locationList);
            var controller = new LocationController(new NullLogger<LocationController>(), mockRepo.Object, null, null);
            // Act
            IActionResult actionResult = controller.Index();
            // Assert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            var locations = Assert.IsAssignableFrom<IEnumerable<LocationViewModel>>(viewResult.Model).ToList();
            Assert.Equal("cali", locations[0].City);
            Assert.Equal(1, locations[0].Id);
        }
        [Fact]
        public void Index_Get_Multiple_Locations()
        {
            // Arrange
            var mockRepo = new Mock<ILocationRepo>();
            var locationList = new List<Location>();
            var cali = new Location();
                cali.Id = 1;
                cali.City = "cali";
            var hk = new Location();
                hk.Id = 2;
                hk.City = "hongkong";
            locationList.Add(cali);
            locationList.Add(hk);
            mockRepo.Setup(r => r.GetAllLocations())
                .Returns
                    (locationList);
            var controller = new LocationController(new NullLogger<LocationController>(), mockRepo.Object, null, null);
            // Act
            IActionResult actionResult = controller.Index();
            // Assert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            var locations = Assert.IsAssignableFrom<IEnumerable<LocationViewModel>>(viewResult.Model).ToList();
            Assert.Equal(2, locations.Count()); // Number of objects in location list
            Assert.Equal(2, locations[1].Id); // Return Id of the second location (hk)
        }
        [Fact]
        public void Index_Get_Valid_Location()
        {
            // Arrange
            var mockRepo = new Mock<ILocationRepo>();
            var locationList = new List<Location>();
            var cali = new Location();
            cali.Id = 1;
            locationList.Add(cali);
            mockRepo.Setup(r => r.GetAllLocations())
                .Returns
                    (locationList);
            var controller = new LocationController(new NullLogger<LocationController>(), mockRepo.Object, null, null);
            // Act
            IActionResult actionResult = controller.Index();
            // Assert
            Assert.True(controller.ModelState.IsValid);
        }
        [Fact]
        public void Index_Create_Location()
        {
            // Arrange
            var mockRepo = new Mock<ILocationRepo>();
            var hk = new Location();
                hk.Id = 1;
                hk.City = "hongkong";
            var controller = new LocationController(new NullLogger<LocationController>(), mockRepo.Object, null, null);
            // Act
            controller.Create(hk);
            // Assert
            Assert.True(controller.ModelState.IsValid);
        }
    }
}
