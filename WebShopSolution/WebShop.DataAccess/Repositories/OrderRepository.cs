
using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.DataAccess.Repositories.Interfaces.WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Entities;

namespace WebShop.DataAccess.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(WebShopDbContext context) : base(context)
        {
        }

        // Add any additional methods specific to Product if needed
        public Order GetMostRecentOrder(int orderId)
        {
            throw new NotImplementedException();
        }
    }

}
