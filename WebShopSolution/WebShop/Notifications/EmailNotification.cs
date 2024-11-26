using WebShop.Entities;
using WebShop.UnitOfWork;

namespace WebShop.Notifications
{
    // Observer that uses UnitOfWork to send email notifications
    public class EmailNotification : INotificationObserver
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmailNotification(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async void Update(Product product)
        {
            var customers = await _unitOfWork.Customers.GetAllAsync(); // Assuming GetAllAsync exists
            foreach (var customer in customers)
            {
                Console.WriteLine($"Email sent to {customer.Name}: New product - {product.Name}");
            }
        }

    }
}

