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
    public class OrderTests
    {
        [Fact]
        public void Index_Get_Orders()
        {
            // Arrange
            Mock<IOrderRepo> mockRepo = new Mock<IOrderRepo>();
            List<Order> orderList = new List<Order>();
            Order firstOrder = new Order
            {
                Location = new Location()
                {
                    Id = 1,
                    City = "nyc"
                },
                Customer = new Customer()
                {
                    Id = 5,
                    LastName = "lastName",
                    FirstName = "firstName",
                    Email = "test@email.com"
                },
                Total = 150
            };
            orderList.Add(firstOrder);
            mockRepo.Setup(r => r.GetAllOrders())
                .Returns
                    (orderList);
            OrderController controller = new OrderController(new NullLogger<OrderController>(), null, null, mockRepo.Object, null);
            // Act
            IActionResult actionResult = controller.Index(null);
            // Assert
            ViewResult viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            List<OrderViewModel> orders = Assert.IsAssignableFrom<IEnumerable<OrderViewModel>>(viewResult.Model).ToList();
            Assert.Equal("test@email.com", orders[0].CustomerEmail);
            Assert.Equal("nyc", orders[0].LocationCity);
        }
        [Fact]
        public void Index_Get_Valid_Order_Search()
        {
            // Arrange
            Mock<IOrderRepo> mockRepo = new Mock<IOrderRepo>();
            List<Order> orderList = new List<Order>();
            Order firstOrder = new Order
            {
                Location = new Location()
                {
                    Id = 1,
                    City = "nyc"
                },
                Customer = new Customer()
                {
                    Id = 5,
                    LastName = "lastName",
                    FirstName = "firstName",
                    Email = "test@email.com"
                },
                Total = 150
            };
            orderList.Add(firstOrder);
            mockRepo.Setup(r => r.GetAllOrders())
                .Returns
                    (orderList);
            OrderController controller = new OrderController(new NullLogger<OrderController>(), null, null, mockRepo.Object, null);
            // Act
            string searchString = "test@email.com";
            IActionResult actionResult = controller.Index(searchString); // Search input
            // Assert
            ViewResult viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            List<OrderViewModel> orders = Assert.IsAssignableFrom<IEnumerable<OrderViewModel>>(viewResult.Model).ToList();
            Assert.Equal("test@email.com", orders[0].CustomerEmail);
        }
        [Fact]
        public void Index_Fail_Order_Search()
        {
            // Arrange
            Mock<IOrderRepo> mockRepo = new Mock<IOrderRepo>();
            List<Order> orderList = new List<Order>();
            Order firstOrder = new Order
            {
                Location = new Location()
                {
                    Id = 1,
                    City = "nyc"
                },
                Customer = new Customer()
                {
                    Id = 5,
                    LastName = "lastName",
                    FirstName = "firstName",
                    Email = "test@email.com"
                },
                Total = 150
            };
            orderList.Add(firstOrder);
            mockRepo.Setup(r => r.GetAllOrders())
                .Returns
                    (orderList);
            OrderController controller = new OrderController(new NullLogger<OrderController>(), null, null, mockRepo.Object, null);
            // Act
            string searchString = "t@gmail.com";
            IActionResult actionResult = controller.Index(searchString); // Search input
            // Assert
            ViewResult viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            List<OrderViewModel> orders = Assert.IsAssignableFrom<IEnumerable<OrderViewModel>>(viewResult.Model).ToList();
            Assert.NotInRange(1, 2, orders.Count()); // Returns no orders since no match to search string
        }
    }
}
