using WebShop.Entities;

namespace WebShop.UnitOfWork
{
    // Gränssnitt för Unit of Work
    public interface IUnitOfWork : IDisposable
    {
         // Repository för produkter
         // Sparar förändringar (om du använder en databas)
        void NotifyProductAdded(Product product); // Notifierar observatörer om ny produkt
    }
}

