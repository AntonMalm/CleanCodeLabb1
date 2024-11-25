using WebShop.DataAccess.Repositories.Interfaces.WebShop.DataAccess.Repositories.Interfaces;
using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Entities;

namespace WebShop.UnitOfWork
{
    // Gränssnitt för Unit of Work
    public interface IUnitOfWork : IDisposable
    {
        // Repository för produkter
        IProductRepository Products { get; }

        // Repository för beställningar
        IOrderRepository Orders { get; }

        // Repository för kunder
        ICustomerRepository Customers { get; }

        // Metod för att spara förändringar (om du använder en databas)
        Task SaveChangesAsync();

        // Metod för att notifiera observatörer om en ny produkt
        void NotifyProductAdded(Product product);
    }
}


