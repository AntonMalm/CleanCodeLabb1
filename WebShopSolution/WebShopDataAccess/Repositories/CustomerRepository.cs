using WebShopDataAccess.Entities;
using WebShopDataAccess.Repositories.Interfaces;

namespace WebShopDataAccess.Repositories
{
    public class CustomerRepository(WebShopDbContext context) : Repository<Customer>(context), ICustomerRepository
    {
        private readonly WebShopDbContext _context = context;

        public IEnumerable<Customer> GetCustomerBySpecificCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentException("Country cannot be null or empty.", nameof(country));

            return _context.Set<Customer>()
                .Where(customer => customer.Country == country)
                .ToList();

        }
    }
}
