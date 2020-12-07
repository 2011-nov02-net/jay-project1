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
            Mock<ICustomerRepo> mockRepo = new Mock<ICustomerRepo>();
            List<Customer> custList = new List<Customer>();
            Customer firstCust = new Customer()
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
            CustomerController controller = new CustomerController(new NullLogger<CustomerController>(), mockRepo.Object, null);
            // Act
            IActionResult actionResult = controller.Index(null);
            // Assert
            ViewResult viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            List<CustomerViewModel> customers = Assert.IsAssignableFrom<IEnumerable<CustomerViewModel>>(viewResult.Model).ToList();
            Assert.Equal("test@email.com", customers[0].Email);
            Assert.Equal(5, customers[0].Id);
        }
        [Fact]
        public void Index_Search_Customers()
        {
            // Arrange
            Mock<ICustomerRepo> mockRepo = new Mock<ICustomerRepo>();
            List<Customer> custList = new List<Customer>();
            Customer firstCust = new Customer()
            {
                Id = 5,
                LastName = "lastName",
                FirstName = "firstName",
                Email = "test@email.com"
            };
            Customer secondCust = new Customer()
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
            CustomerController controller = new CustomerController(new NullLogger<CustomerController>(), mockRepo.Object, null);
            // Act
            string searchString = "lastName2";
            IActionResult actionResult = controller.Index(searchString);
            // Assert
            ViewResult viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            List<CustomerViewModel> customers = Assert.IsAssignableFrom<IEnumerable<CustomerViewModel>>(viewResult.Model).ToList();
            Assert.Equal("lastName2", customers[0].LastName);
            Assert.Equal(8, customers[0].Id);
        }
        [Fact]
        public void Create_Valid_Customer()
        {
            // Arrange
            Mock<ICustomerRepo> mockRepo = new Mock<ICustomerRepo>();
            Customer firstCust = new Customer()
            {
                Id = 5,
                LastName = "lastName",
                FirstName = "firstName",
                Email = "test@email.com"
            };
            CustomerController controller = new CustomerController(new NullLogger<CustomerController>(), mockRepo.Object, null);
            // Act
            IActionResult actionResult = controller.Create(firstCust);
            // Assert
            Assert.True(controller.ModelState.IsValid);
        }
    }
}
