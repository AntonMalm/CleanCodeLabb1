namespace WebShop.Payments
{
    public interface IPaymentProcessor
    {
        string ProcessPayment(decimal amount, IPaymentMethod paymentMethod);
    }
}

