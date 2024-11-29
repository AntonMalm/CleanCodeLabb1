using WebShop.DataAccess.Entities;

namespace WebShop.DataAccess.Repositories.Interfaces
{

    namespace WebShop.DataAccess.Repositories.Interfaces
    {
        public interface IProductRepository : IRepository<Product>
        {
            public Product GetCheapestProduct();
        }
    }

}
