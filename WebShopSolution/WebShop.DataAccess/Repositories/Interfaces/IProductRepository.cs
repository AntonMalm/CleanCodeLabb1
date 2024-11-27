using WebShop.Entities;

namespace WebShop.DataAccess.Repositories.Interfaces
{
    // Gränssnitt för produktrepositoryt enligt Repository Pattern

    namespace WebShop.DataAccess.Repositories.Interfaces
    {
        public interface IProductRepository : IRepository<Product>
        {
            public Product GetCheapestProduct();
        }
    }

}
