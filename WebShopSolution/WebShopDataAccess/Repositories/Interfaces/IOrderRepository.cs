using WebShopDataAccess.Entities;

namespace WebShopDataAccess.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order GetMostRecentOrder ();
    }
}
