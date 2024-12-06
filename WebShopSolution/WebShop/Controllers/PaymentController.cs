using Microsoft.AspNetCore.Mvc;
using WebShop.Payments;
using WebShop.UnitOfWork;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("process-payment")]
        public IActionResult ProcessPayment(decimal amount)
        {
            var paymentMethod = new SwishPayment();

            var processor = new PaymentProcessor(paymentMethod);

            var result = processor.ProcessPayment(amount, paymentMethod);

            return Ok(result);
        }
    }
}