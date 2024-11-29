using WebShop.DataAccess.Entities;

namespace WebShop.DataAccess.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        IEnumerable<Customer> GetCustomerBySpecificCountry(string country);
    }
}
