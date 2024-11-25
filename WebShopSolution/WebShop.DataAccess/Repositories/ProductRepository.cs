﻿

using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.DataAccess.Repositories.Interfaces.WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Entities;

namespace WebShop.DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(WebShopDbContext context) : base(context)
        {
        }

        // Add any additional methods specific to Product if needed
    }
}
