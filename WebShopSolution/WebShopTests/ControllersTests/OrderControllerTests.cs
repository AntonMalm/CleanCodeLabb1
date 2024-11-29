using Microsoft.AspNetCore.Mvc;
using Moq;
using WebShop.Controllers;
using WebShop.DataAccess.Entities;
using WebShop.DTOs;
using WebShop.UnitOfWork;
namespace WebShopTests.ControllersTests;
public class OrderControllerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly OrderController _controller;

    public OrderControllerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _controller = new OrderController(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetOrders_ReturnsOkResult_WithAListOfOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order { Id = 1, CustomerId = 1, TotalPrice = 100 },
            new Order { Id = 2, CustomerId = 2, TotalPrice = 200 }
        };

        _mockUnitOfWork.Setup(u => u.Orders.GetAllAsync()).ReturnsAsync(orders);

        // Act
        var result = await _controller.GetOrders();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<Order>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetOrder_ReturnsOkResult_WithOrder()
    {
        // Arrange
        var order = new Order { Id = 1, CustomerId = 1, TotalPrice = 100 };

        _mockUnitOfWork.Setup(u => u.Orders.GetByIdAsync(1)).ReturnsAsync(order);

        // Act
        var result = await _controller.GetOrder(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Order>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public async Task GetOrder_ReturnsNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.Orders.GetByIdAsync(1)).ReturnsAsync((Order)null);

        // Act
        var result = await _controller.GetOrder(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task AddOrder_ReturnsCreatedAtAction_WhenOrderIsAdded()
    {
        // Arrange
        var orderDto = new OrderCreateDTO
        {
            CustomerId = 1,
            TotalPrice = 150,
            OrderItems = new List<OrderItemCreateDTO>
            {
                new OrderItemCreateDTO { ProductId = 1, Quantity = 2, Price = 50 },
                new OrderItemCreateDTO { ProductId = 2, Quantity = 1, Price = 50 }
            }
        };

        var customer = new Customer { Id = 1, Name = "Test Customer" };
        var product = new Product { Id = 1, Name = "Test Product", Price = 50 };

        _mockUnitOfWork.Setup(u => u.Customers.GetByIdAsync(1)).ReturnsAsync(customer);
        _mockUnitOfWork.Setup(u => u.Products.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(product);
        _mockUnitOfWork.Setup(u => u.Orders.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddOrder(orderDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(OrderController.GetOrder), createdResult.ActionName);
    }

    [Fact]
    public async Task AddOrder_ReturnsBadRequest_WhenCustomerIdIsInvalid()
    {
        // Arrange
        var orderDto = new OrderCreateDTO { CustomerId = 99, TotalPrice = 100 };
        _mockUnitOfWork.Setup(u => u.Customers.GetByIdAsync(99)).ReturnsAsync((Customer)null);

        // Act
        var result = await _controller.AddOrder(orderDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid CustomerId", badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteOrder_ReturnsNoContent_WhenOrderIsDeleted()
    {
        // Arrange
        var order = new Order { Id = 1, CustomerId = 1, TotalPrice = 100 };
        _mockUnitOfWork.Setup(u => u.Orders.GetByIdAsync(1)).ReturnsAsync(order);
        _mockUnitOfWork.Setup(u => u.Orders.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteOrder(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteOrder_ReturnsNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.Orders.GetByIdAsync(1)).ReturnsAsync((Order)null);

        // Act
        var result = await _controller.DeleteOrder(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetMostRecentOrder_ReturnsOkResult_WithOrder()
    {
        // Arrange
        var order = new Order { Id = 5, CustomerId = 1, TotalPrice = 300 };
        _mockUnitOfWork.Setup(u => u.Orders.GetMostRecentOrder()).Returns(order);

        // Act
        var result = await _controller.GetMostRecentOrder();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Order>(okResult.Value);
        Assert.Equal(5, returnValue.Id);
    }
}
