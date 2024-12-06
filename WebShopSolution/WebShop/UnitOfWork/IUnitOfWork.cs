using WebShop.Payments;
using WebShopDataAccess.Entities;
using WebShopDataAccess.Repositories.Interfaces;
using WebShopDataAccess.Repositories.Interfaces.WebShopDataAccess.Repositories.Interfaces;

namespace WebShop.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        ICustomerRepository Customers { get; }
        IPaymentMethod PaymentMethod { get; }
        Task SaveChangesAsync();
        void NotifyProductAdded(Product product);
    }
}