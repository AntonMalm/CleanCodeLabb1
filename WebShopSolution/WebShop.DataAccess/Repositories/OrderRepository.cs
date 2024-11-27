
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.DataAccess.Repositories.Interfaces.WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Entities;

namespace WebShop.DataAccess.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly WebShopDbContext _context;
        public OrderRepository(WebShopDbContext context) : base(context)
        {
            _context = context;
        }

        public Order GetMostRecentOrder()
        {
            var mostRecentOrder = _context.Set<Order>()
                .OrderByDescending(order => order.Id)
                .FirstOrDefault();

            if (mostRecentOrder is null)
                return null;

            return mostRecentOrder;
        }

    }

}
