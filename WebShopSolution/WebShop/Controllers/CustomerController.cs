using Microsoft.AspNetCore.Mvc;
using WebShop.UnitOfWork;
using WebShopDataAccess.Entities;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await unitOfWork.Customers.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            try
            {
                var customer = await unitOfWork.Customers.GetByIdAsync(id);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddCustomer([FromBody] Customer customer)
        {
            if (customer == null)
                return BadRequest(new { message = "Invalid customer data." });

            await unitOfWork.Customers.AddAsync(customer);
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
        {
            if (updatedCustomer == null || updatedCustomer.Id != id)
                return BadRequest(new { message = "Customer data is invalid." });

            try
            {
                await unitOfWork.Customers.UpdateAsync(updatedCustomer);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            try
            {
                await unitOfWork.Customers.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("by-country/{country}")]
        public ActionResult<IEnumerable<Customer>> GetCustomersByCountry(string country)
        {
            var customers = unitOfWork.Customers.GetCustomerBySpecificCountry(country);
            return Ok(customers);
        }
    }
}
