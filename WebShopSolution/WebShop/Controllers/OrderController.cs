using Microsoft.AspNetCore.Mvc;
using WebShop.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.DataAccess.Entities;
using WebShop.DTOs;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await unitOfWork.Orders.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrder(OrderCreateDTO orderDto)
        {
            var customer = await unitOfWork.Customers.GetByIdAsync(orderDto.CustomerId);
            if (customer == null)
            {
                return BadRequest("Invalid CustomerId");
            }

            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                TotalPrice = orderDto.TotalPrice,
                OrderItems = new List<OrderItem>()
            };

            foreach (var orderItemDto in orderDto.OrderItems)
            {
                var product = await unitOfWork.Products.GetByIdAsync(orderItemDto.ProductId);
                if (product == null)
                {
                    return BadRequest($"Invalid ProductId: {orderItemDto.ProductId}");
                }

                var orderItem = new OrderItem
                {
                    ProductId = orderItemDto.ProductId,
                    Quantity = orderItemDto.Quantity,
                    Price = orderItemDto.Price,
                    Order = order
                };

                order.OrderItems.Add(orderItem);
            }

            await unitOfWork.Orders.AddAsync(order);
            await unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] Order updatedOrder)
        {
            if (updatedOrder == null || updatedOrder.Id != id)
                return BadRequest(new { message = "Order data is invalid." });

            try
            {
                await unitOfWork.Orders.UpdateAsync(updatedOrder);

                await unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                var order = await unitOfWork.Orders.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new { message = "Order not found" });
                }

                await unitOfWork.Orders.DeleteAsync(id);
                await unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("most-recent")]
        public async Task<ActionResult<Order>> GetMostRecentOrder()
        {
            var order = unitOfWork.Orders.GetMostRecentOrder();
            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }
            return Ok(order);
        }
    }
}
