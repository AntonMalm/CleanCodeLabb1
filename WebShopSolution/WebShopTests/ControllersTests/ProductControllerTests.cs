using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebShop.Controllers;
using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.DataAccess.Repositories.Interfaces.WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Entities;
using WebShop.UnitOfWork;
namespace WebShopTests.ControllersTests;


public class ProductControllerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        // Mock the IProductRepository
        _mockProductRepository = new Mock<IProductRepository>();

        // Mock the IUnitOfWork
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);

        // Create the ProductController with the mocked UnitOfWork
        _controller = new ProductController(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithAListOfProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product1" },
            new Product { Id = 2, Name = "Product2" }
        };
        _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsType<List<Product>>(okResult.Value);
        Assert.Equal(2, returnedProducts.Count);
    }

    [Fact]
    public async Task GetProduct_ReturnsOkResult_WithProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product1" };
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _controller.GetProduct(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProduct = Assert.IsType<Product>(okResult.Value);
        Assert.Equal(1, returnedProduct.Id);
    }

    [Fact]
    public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Product)null);

        // Act
        var result = await _controller.GetProduct(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains("Product not found", notFoundResult.Value.ToString());
    }

    [Fact]
    public async Task AddProduct_ReturnsCreatedAtAction_WithProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product1" };
        _mockProductRepository.Setup(repo => repo.AddAsync(product)).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddProduct(product);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedProduct = Assert.IsType<Product>(createdAtActionResult.Value);
        Assert.Equal(product.Id, returnedProduct.Id);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "UpdatedProduct" };
        _mockProductRepository.Setup(repo => repo.UpdateAsync(product)).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateProduct(1, product);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var product = new Product { Id = 2, Name = "UpdatedProduct" };

        // Act
        var result = await _controller.UpdateProduct(1, product);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Product data is invalid", badRequestResult.Value.ToString());
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product1" };
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);
        _mockProductRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Product)null);

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("Product not found", notFoundResult.Value.ToString());
    }

    [Fact]
    public void GetCheapestProduct_ReturnsOkResult_WithProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "CheapestProduct" };
        _mockProductRepository.Setup(repo => repo.GetCheapestProduct()).Returns(product);

        // Act
        var result = _controller.GetCheapestProduct();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProduct = Assert.IsType<Product>(okResult.Value);
        Assert.Equal(product.Name, returnedProduct.Name);
    }
}
