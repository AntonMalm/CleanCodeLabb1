

using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.DataAccess.Repositories.Interfaces.WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Entities;

namespace WebShop.DataAccess.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(WebShopDbContext context) : base(context)
        {
        }
        public Customer GetCustomerBySpecificCountry(string country)
        {
            throw new NotImplementedException();
        }
        // Add any additional methods specific to Product if needed
    }
}
