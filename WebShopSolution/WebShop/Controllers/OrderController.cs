using Microsoft.AspNetCore.Mvc;
using WebShop.Entities;
using WebShop.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.DTOs;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor injection of IUnitOfWork
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Endpoint for getting all orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return Ok(orders);
        }

        // Endpoint for getting a specific order by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }
            return Ok(order);
        }

        // Endpoint for adding a new order
        [HttpPost]
        public async Task<ActionResult> AddOrder(OrderCreateDTO orderDto)
        {
            // Validate CustomerId
            var customer = await _unitOfWork.Customers.GetByIdAsync(orderDto.CustomerId);
            if (customer == null)
            {
                return BadRequest("Invalid CustomerId");
            }

            // Create Order
            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                TotalPrice = orderDto.TotalPrice,
                OrderItems = new List<OrderItem>()
            };

            // Add OrderItems
            foreach (var orderItemDto in orderDto.OrderItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(orderItemDto.ProductId);
                if (product == null)
                {
                    return BadRequest($"Invalid ProductId: {orderItemDto.ProductId}");
                }

                var orderItem = new OrderItem
                {
                    ProductId = orderItemDto.ProductId,
                    Quantity = orderItemDto.Quantity,
                    Price = orderItemDto.Price,
                    Order = order  // This will link the Order with the OrderItem
                };

                order.OrderItems.Add(orderItem);
            }

            // Add the Order to the database
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        // Endpoint for updating an existing order
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] Order updatedOrder)
        {
            if (updatedOrder == null || updatedOrder.Id != id)
                return BadRequest(new { message = "Order data is invalid." });

            try
            {
                // Update order via repository
                await _unitOfWork.Orders.UpdateAsync(updatedOrder);

                // Save changes in the database
                await _unitOfWork.SaveChangesAsync();

                return NoContent(); // Successful update with no content to return
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Endpoint for deleting an order
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new { message = "Order not found" });
                }

                await _unitOfWork.Orders.DeleteAsync(id);
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
