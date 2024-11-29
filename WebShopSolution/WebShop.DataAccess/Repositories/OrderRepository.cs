using WebShop.DataAccess.Entities;
using WebShop.DataAccess.Repositories.Interfaces;

namespace WebShop.DataAccess.Repositories
{
    public class OrderRepository(WebShopDbContext context) : Repository<Order>(context), IOrderRepository
    {
        private readonly WebShopDbContext _context = context;

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
