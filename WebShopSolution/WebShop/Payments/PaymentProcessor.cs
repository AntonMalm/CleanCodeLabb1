using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Payments;

using WebShop.Payments;

namespace WebShop.Payments
{
    public class PaymentProcessor : IPaymentProcessor
    {
        private readonly IPaymentMethod _paymentMethod;

        public PaymentProcessor(IPaymentMethod paymentMethod)
        {
            _paymentMethod = paymentMethod;
        }

        public string ProcessPayment(decimal amount, IPaymentMethod paymentMethod)
        {
            return paymentMethod.Pay(amount);
        }
    }
}