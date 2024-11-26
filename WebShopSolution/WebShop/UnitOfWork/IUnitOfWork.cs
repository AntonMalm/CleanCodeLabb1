using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.DataAccess.Repositories.Interfaces.WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Entities;

namespace WebShop.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        ICustomerRepository Customers { get; }
        Task SaveChangesAsync();
        void NotifyProductAdded(Product product);
    }
}

