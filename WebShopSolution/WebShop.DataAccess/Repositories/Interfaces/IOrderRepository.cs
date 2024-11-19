using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Entities;

namespace WebShop.DataAccess.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Order GetMostRecentOrder (int orderId);
    }
}
