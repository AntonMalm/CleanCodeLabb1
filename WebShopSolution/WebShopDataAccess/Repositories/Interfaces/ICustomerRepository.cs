using WebShopDataAccess.Entities;

namespace WebShopDataAccess.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        IEnumerable<Customer> GetCustomerBySpecificCountry(string country);
    }
}
