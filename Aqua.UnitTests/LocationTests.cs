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
        }
    }
}
