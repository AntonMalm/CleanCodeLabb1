using WebShop.UnitOfWork;
using WebShopDataAccess.Entities;

namespace WebShop.Notifications
{
    public class EmailNotification : INotificationObserver
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmailNotification(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async void Update(Product product)
        {
            var customers = await _unitOfWork.Customers.GetAllAsync();
            foreach (var customer in customers)
            {
                Console.WriteLine($"Email sent to {customer.Name}: New product - {product.Name}");
            }
        }

    }
}

