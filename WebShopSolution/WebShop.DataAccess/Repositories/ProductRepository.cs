

using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.DataAccess.Repositories.Interfaces.WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Entities;

namespace WebShop.DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly WebShopDbContext _context;
        public ProductRepository(WebShopDbContext context) : base(context)
        {
            _context = context;
        }

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
