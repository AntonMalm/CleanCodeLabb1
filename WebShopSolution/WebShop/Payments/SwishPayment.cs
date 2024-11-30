using WebShop.DataAccess.Repositories.Interfaces;

namespace WebShop.DataAccess.Payments
{
    public class SwishPayment : IPaymentMethod
    {
        public string Pay(decimal amount)
        {
            return $"Payment of {amount} successful with Swish";
        }
    }
}
