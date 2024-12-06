namespace WebShop.Payments;

public interface IPaymentMethod
{
    string Pay(decimal amount);
}
