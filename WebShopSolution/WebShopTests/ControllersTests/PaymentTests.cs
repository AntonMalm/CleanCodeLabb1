using WebShop.DataAccess.Payments;
using WebShop.Payments;


namespace WebShopTests.ControllersTests;
public class PaymentTests
{
    [Fact]
    public void ProcessPayment_ReturnsSuccess_WhenUsingSwish()
    {
        // Arrange
        var paymentProcessor = new PaymentProcessor(new SwishPayment());

        // Act
        var result = paymentProcessor.ProcessPayment(200, new SwishPayment());

        // Assert
        Assert.Equal("Payment of 200 successful with Swish", result);
    }
}

