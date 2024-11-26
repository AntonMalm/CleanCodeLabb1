using Moq;
using WebShop.Entities;
using WebShop.Notifications;
using WebShop.UnitOfWork;
using WebShop.DataAccess.Repositories.Interfaces;
using Xunit;
using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess.Repositories.Interfaces.WebShop.DataAccess.Repositories.Interfaces;
using WebShop.DataAccess;

namespace WebShop.Tests
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void NotifyProductAdded_CallsObserverUpdate()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test" };

            // Create a mock of INotificationObserver
            var mockObserver = new Mock<INotificationObserver>();

            // Create an instance of ProductSubject and add the mock observer
            var productSubject = new ProductSubject();
            productSubject.Attach(mockObserver.Object);

            // Create mocks for the repositories
            var mockProductRepository = new Mock<IProductRepository>();
            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockCustomerRepository = new Mock<ICustomerRepository>();

            // Create a mock of WebShopDbContext
            var mockContext = new Mock<WebShopDbContext>(new DbContextOptions<WebShopDbContext>());

            // Inject the ProductSubject into UnitOfWork
            var unitOfWork = new UnitOfWork.UnitOfWork(
                mockProductRepository.Object,
                mockOrderRepository.Object,
                mockCustomerRepository.Object,
                mockContext.Object,
                productSubject
            );

            // Act
            unitOfWork.NotifyProductAdded(product);

            // Assert
            // Verify that the Update method was called on the mock observer
            mockObserver.Verify(o => o.Update(product), Times.Once);
        }
    }
}