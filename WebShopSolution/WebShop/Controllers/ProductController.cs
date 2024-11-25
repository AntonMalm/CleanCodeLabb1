using Microsoft.AspNetCore.Mvc;
using WebShop.Entities;
using WebShop.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor injection of IUnitOfWork
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Endpoint för att hämta alla produkter
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // Behöver använda repository via Unit of Work för att hämta produkter
            var products = await _unitOfWork.Products.GetAllAsync();
            return Ok(products);
        }

        // Endpoint för att hämta en produkt med ett specifikt ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                // Hämtar produkt via repository
                var product = await _unitOfWork.Products.GetByIdAsync(id);
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

        // Endpoint för att lägga till en ny produkt
        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            if (product == null)
                return BadRequest(new { message = "Product data is invalid." });

            // Lägger till produkten via repository
            await _unitOfWork.Products.AddAsync(product);

            // Sparar förändringar i databasen
            await _unitOfWork.SaveChangesAsync();

            // Notifierar observatörer om att en ny produkt har lagts till
            _unitOfWork.NotifyProductAdded(product);

            // Returnerar en "Created" status med platsen för den nyskapade produkten
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // Endpoint för att uppdatera en produkt
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null || updatedProduct.Id != id)
                return BadRequest(new { message = "Product data is invalid." });

            try
            {
                // Uppdaterar produkt via repository
                await _unitOfWork.Products.UpdateAsync(updatedProduct);

                // Sparar förändringar i databasen
                await _unitOfWork.SaveChangesAsync();

                return NoContent(); // Successful update with no content to return
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Endpoint för att ta bort en produkt
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                // Hämtar produkt och tar bort den via repository
                var product = await _unitOfWork.Products.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { message = "Product not found" });
                }

                await _unitOfWork.Products.DeleteAsync(id);

                // Sparar förändringar i databasen
                await _unitOfWork.SaveChangesAsync();

                return NoContent(); // Successful deletion with no content to return
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
