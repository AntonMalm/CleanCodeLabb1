using WebShop.DataAccess;
using WebShop.DataAccess.Entities;
using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.DataAccess.Repositories.Interfaces.WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Notifications;

namespace WebShop.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IProductRepository Products { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public ICustomerRepository Customers { get; private set; }

        private readonly ProductSubject _productSubject;
        private readonly WebShopDbContext _context; // Assuming you're using Entity Framework Core

        public UnitOfWork(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            WebShopDbContext context,
            ProductSubject productSubject = null)
        {
            Products = productRepository;
            Orders = orderRepository;
            Customers = customerRepository;
            _context = context;
            _productSubject = productSubject ?? new ProductSubject();
            _productSubject.Attach(new EmailNotification(this));
        }


        // Method to notify observers when a product is added
        public void NotifyProductAdded(Product product)
        {
            _productSubject.Notify(product);
        }

        // Method to save changes to the database
        public async Task SaveChangesAsync()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Dispose method for cleanup
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
