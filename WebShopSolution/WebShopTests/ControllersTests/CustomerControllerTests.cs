using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Controllers;
using WebShop.Entities;
using WebShop.UnitOfWork;
using Xunit;

namespace WebShopTests.ControllersTests
{
    public class CustomerControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new CustomerController(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetCustomers_ReturnsAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "John Doe" },
                new Customer { Id = 2, Name = "Jane Smith" }
            };
            _mockUnitOfWork.Setup(u => u.Customers.GetAllAsync()).ReturnsAsync(customers);

            // Act
            var result = await _controller.GetCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCustomers = Assert.IsType<List<Customer>>(okResult.Value);
            Assert.Equal(2, returnedCustomers.Count);
        }

        [Fact]
        public async Task GetCustomer_ReturnsCustomer_WhenIdExists()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "John Doe" };
            _mockUnitOfWork.Setup(u => u.Customers.GetByIdAsync(1)).ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomer(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCustomer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(1, returnedCustomer.Id);
            Assert.Equal("John Doe", returnedCustomer.Name);
        }

        [Fact]
        public async Task AddCustomer_CreatesNewCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "John Doe" };
            _mockUnitOfWork.Setup(u => u.Customers.AddAsync(customer)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddCustomer(customer);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedCustomer = Assert.IsType<Customer>(createdAtResult.Value);
            Assert.Equal(1, returnedCustomer.Id);
            Assert.Equal("John Doe", returnedCustomer.Name);
        }

        [Fact]
        public async Task UpdateCustomer_UpdatesExistingCustomer()
        {
            // Arrange
            var updatedCustomer = new Customer { Id = 1, Name = "John Updated" };
            _mockUnitOfWork.Setup(u => u.Customers.UpdateAsync(updatedCustomer)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateCustomer(1, updatedCustomer);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_RemovesCustomer()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Customers.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCustomer(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void GetCustomersByCountry_ReturnsCustomersInSpecificCountry()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "John Doe", Country = "USA" },
                new Customer { Id = 2, Name = "Jane Smith", Country = "USA" }
            };
            _mockUnitOfWork.Setup(u => u.Customers.GetCustomerBySpecificCountry("USA")).Returns(customers);

            // Act
            var result = _controller.GetCustomersByCountry("USA");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCustomers = Assert.IsType<List<Customer>>(okResult.Value);
            Assert.Equal(2, returnedCustomers.Count);
        }
    }
}
