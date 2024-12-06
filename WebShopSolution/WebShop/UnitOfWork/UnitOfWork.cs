using WebShop.Notifications;
using WebShop.Payments;
using WebShopDataAccess;
using WebShopDataAccess.Entities;
using WebShopDataAccess.Repositories.Interfaces;
using WebShopDataAccess.Repositories.Interfaces.WebShopDataAccess.Repositories.Interfaces;

namespace WebShop.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IProductRepository Products { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public ICustomerRepository Customers { get; private set; }
        public IPaymentMethod PaymentMethod { get; private set; }

        private readonly ProductSubject _productSubject;
        private readonly WebShopDbContext _context;

        public UnitOfWork(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IPaymentMethod paymentMethod,
            WebShopDbContext context,
            ProductSubject productSubject = null)
        {
            Products = productRepository;
            Orders = orderRepository;
            Customers = customerRepository;
            PaymentMethod = paymentMethod;
            _context = context;
            _productSubject = productSubject ?? new ProductSubject();
            _productSubject.Attach(new EmailNotification(this));
        }

        public void NotifyProductAdded(Product product)
        {
            _productSubject.Notify(product);
        }

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

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
