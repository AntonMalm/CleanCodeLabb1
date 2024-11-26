

using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.DataAccess.Repositories.Interfaces.WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Entities;

namespace WebShop.DataAccess.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly WebShopDbContext _context;

        public CustomerRepository(WebShopDbContext context) : base(context)
        {
            _context = context;
        }

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
