using Microsoft.EntityFrameworkCore;
using Moq;
using WebShop.Notifications;
using WebShop.Payments;
using WebShopDataAccess;
using WebShopDataAccess.Entities;
using WebShopDataAccess.Repositories.Interfaces;
using WebShopDataAccess.Repositories.Interfaces.WebShopDataAccess.Repositories.Interfaces;

namespace WebShopTests
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void NotifyProductAdded_CallsObserverUpdate()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test" };

            var mockObserver = new Mock<INotificationObserver>();
            var productSubject = new ProductSubject();
            productSubject.Attach(mockObserver.Object);

            var mockProductRepository = new Mock<IProductRepository>();
            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockCustomerRepository = new Mock<ICustomerRepository>();

            var mockPaymentMethod = new Mock<IPaymentMethod>();

            var mockContext = new Mock<WebShopDbContext>(new DbContextOptions<WebShopDbContext>());

            var unitOfWork = new WebShop.UnitOfWork.UnitOfWork(
                mockProductRepository.Object,
                mockOrderRepository.Object,
                mockCustomerRepository.Object,
                mockPaymentMethod.Object,
                mockContext.Object,
                productSubject
            );

            // Act
            unitOfWork.NotifyProductAdded(product);

            // Assert
            mockObserver.Verify(o => o.Update(product), Times.Once);
        }
    }
}