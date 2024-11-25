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

        // Endpoint f�r att h�mta alla produkter
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // Beh�ver anv�nda repository via Unit of Work f�r att h�mta produkter
            var products = await _unitOfWork.Products.GetAllAsync();
            return Ok(products);
        }

        // Endpoint f�r att h�mta en produkt med ett specifikt ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                // H�mtar produkt via repository
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

        // Endpoint f�r att l�gga till en ny produkt
        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            if (product == null)
                return BadRequest(new { message = "Product data is invalid." });

            // L�gger till produkten via repository
            await _unitOfWork.Products.AddAsync(product);

            // Sparar f�r�ndringar i databasen
            await _unitOfWork.SaveChangesAsync();

            // Notifierar observat�rer om att en ny produkt har lagts till
            _unitOfWork.NotifyProductAdded(product);

            // Returnerar en "Created" status med platsen f�r den nyskapade produkten
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // Endpoint f�r att uppdatera en produkt
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null || updatedProduct.Id != id)
                return BadRequest(new { message = "Product data is invalid." });

            try
            {
                // Uppdaterar produkt via repository
                await _unitOfWork.Products.UpdateAsync(updatedProduct);

                // Sparar f�r�ndringar i databasen
                await _unitOfWork.SaveChangesAsync();

                return NoContent(); // Successful update with no content to return
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Endpoint f�r att ta bort en produkt
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                // H�mtar produkt och tar bort den via repository
                var product = await _unitOfWork.Products.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { message = "Product not found" });
                }

                await _unitOfWork.Products.DeleteAsync(id);

                // Sparar f�r�ndringar i databasen
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
