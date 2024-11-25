using Microsoft.AspNetCore.Mvc;
using WebShop.Entities;
using WebShop.UnitOfWork;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Endpoint to retrieve all customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _unitOfWork.Customers.GetAllAsync();
            return Ok(customers);
        }

        // Endpoint to retrieve a specific customer by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(id);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Endpoint to add a new customer
        [HttpPost]
        public async Task<ActionResult> AddCustomer([FromBody] Customer customer)
        {
            if (customer == null)
                return BadRequest(new { message = "Invalid customer data." });

            await _unitOfWork.Customers.AddAsync(customer);
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        // Endpoint to update an existing customer
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
        {
            if (updatedCustomer == null || updatedCustomer.Id != id)
                return BadRequest(new { message = "Customer data is invalid." });

            try
            {
                await _unitOfWork.Customers.UpdateAsync(updatedCustomer);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Endpoint to delete a customer by ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _unitOfWork.Customers.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Example of a custom method for customers
        [HttpGet("by-country/{country}")]
        public ActionResult<IEnumerable<Customer>> GetCustomersByCountry(string country)
        {
            var customers = _unitOfWork.Customers.GetCustomerBySpecificCountry(country);
            return Ok(customers);
        }
    }
}
