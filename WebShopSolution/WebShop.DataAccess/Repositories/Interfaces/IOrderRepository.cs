using WebShop.DataAccess.Entities;

namespace WebShop.DataAccess.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order GetMostRecentOrder ();
    }
}
