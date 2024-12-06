using WebShopDataAccess.Entities;
using WebShopDataAccess.Repositories.Interfaces.WebShopDataAccess.Repositories.Interfaces;

namespace WebShopDataAccess.Repositories
{
    public class ProductRepository(WebShopDbContext context) : Repository<Product>(context), IProductRepository
    {
        private readonly WebShopDbContext _context = context;

        public Product GetCheapestProduct()
        {
            var cheapestProduct = _context.Set<Product>()
                .OrderBy(product => product.Price)
                .FirstOrDefault();

            if (cheapestProduct is null)
                return null;

            return cheapestProduct;
        }
    }
}
