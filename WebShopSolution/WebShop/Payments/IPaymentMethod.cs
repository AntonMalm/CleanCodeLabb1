using WebShop.Payments;

namespace WebShop.DataAccess.Repositories.Interfaces;

public interface IPaymentMethod
{
    string Pay(decimal amount);
}
