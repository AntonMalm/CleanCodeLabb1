using WebShopDataAccess.Entities;

namespace WebShopDataAccess.Repositories.Interfaces
{

    namespace WebShopDataAccess.Repositories.Interfaces
    {
        public interface IProductRepository : IRepository<Product>
        {
            public Product GetCheapestProduct();
        }
    }

}
