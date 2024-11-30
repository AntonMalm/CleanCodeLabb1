using WebShop.DataAccess.Entities;
using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.DataAccess.Repositories.Interfaces.WebShop.DataAccess.Repositories.Interfaces;

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