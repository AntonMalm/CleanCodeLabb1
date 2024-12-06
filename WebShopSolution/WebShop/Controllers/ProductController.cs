using Microsoft.AspNetCore.Mvc;
using WebShop.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebShopDataAccess.Entities;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await unitOfWork.Products.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var product = await unitOfWork.Products.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { message = "Product not found" });
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            if (product == null)
                return BadRequest(new { message = "Product data is invalid." });

            await unitOfWork.Products.AddAsync(product);

            await unitOfWork.SaveChangesAsync();

            unitOfWork.NotifyProductAdded(product);

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null || updatedProduct.Id != id)
                return BadRequest(new { message = "Product data is invalid." });

            try
            {
                await unitOfWork.Products.UpdateAsync(updatedProduct);

                await unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await unitOfWork.Products.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { message = "Product not found" });
                }

                await unitOfWork.Products.DeleteAsync(id);

                await unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("cheapest-product")]
        public ActionResult<Product> GetCheapestProduct()
        {
            var product = unitOfWork.Products.GetCheapestProduct();
            return Ok(product);
        }
    }
}
