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
    public class CustomerTests
    {
        [Fact]
        public void Index_Get_Customers()
        {
            // Arrange
            var mockRepo = new Mock<ICustomerRepo>();
            var custList = new List<Customer>();
            var firstCust = new Customer()
            {
                Id = 5,
                LastName = "lastName",
                FirstName = "firstName",
                Email = "test@email.com"
            };
            custList.Add(firstCust);
            mockRepo.Setup(r => r.GetAllCustomers())
                .Returns
                    (custList);
            var controller = new CustomerController(new NullLogger<CustomerController>(),  mockRepo.Object, null);
            // Act
            IActionResult actionResult = controller.Index(null);
            // Assert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            var customers = Assert.IsAssignableFrom<IEnumerable<CustomerViewModel>>(viewResult.Model).ToList();
            Assert.Equal("test@email.com", customers[0].Email);
            Assert.Equal(5, customers[0].Id);
        }
        [Fact]
        public void Index_Search_Customers()
        {
            // Arrange
            var mockRepo = new Mock<ICustomerRepo>();
            var custList = new List<Customer>();
            var firstCust = new Customer()
            {
                Id = 5,
                LastName = "lastName",
                FirstName = "firstName",
                Email = "test@email.com"
            };
            var secondCust = new Customer()
            {
                Id = 8,
                LastName = "lastName2",
                FirstName = "firstName2",
                Email = "secondCust@email.com"
            };
            custList.Add(firstCust);
            custList.Add(secondCust);
            mockRepo.Setup(r => r.GetAllCustomers())
                .Returns
                    (custList);
            var controller = new CustomerController(new NullLogger<CustomerController>(), mockRepo.Object, null);
            // Act
            var searchString = "lastName2";
            IActionResult actionResult = controller.Index(searchString);
            // Assert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            var customers = Assert.IsAssignableFrom<IEnumerable<CustomerViewModel>>(viewResult.Model).ToList();
            Assert.Equal("lastName2", customers[0].LastName);
            Assert.Equal(8, customers[0].Id);
        }
        [Fact]
        public void Create_Valid_Customer()
        {
            // Arrange
            var mockRepo = new Mock<ICustomerRepo>();
            var firstCust = new Customer()
            {
                Id = 5,
                LastName = "lastName",
                FirstName = "firstName",
                Email = "test@email.com"
            };
            var controller = new CustomerController(new NullLogger<CustomerController>(), mockRepo.Object, null);
            // Act
            IActionResult actionResult = controller.Create(firstCust);
            // Assert
            Assert.True(controller.ModelState.IsValid);
        }
    }
}
