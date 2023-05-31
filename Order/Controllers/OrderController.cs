using Microsoft.AspNetCore.Mvc;
using ProductOrder.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ProductOrder.Models.Dtos;
using ProductOrder.Enum;
using static NuGet.Packaging.PackagingConstants;

namespace ProductOrder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        ApplicationContext db = new ApplicationContext();

        /// <summary>
        /// Возвращает из базы данных все заказы со списком содержимого из базы данных
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var orders = await db.Orders.Include(p => p.OrderItems).ToListAsync();

            if (!orders.Any())
            {
                return NotFound();
            }

            var ordersDto = orders.Select(p => new OrderDto
            {
                Id = p.Id,
                CustomerId = p.Id,
                OrderDate = p.OrderDate,
                OrderItemsDto = ConverItemToDto(p.OrderItems),
                OrderStatus = p.OrderStatus,
                ShippingAddressId = p.ShippingAddressId,
                TotalAmount = p.TotalAmount
            });

            return Ok(ordersDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var order = await db.Orders.Include(p => p.OrderItems).SingleOrDefaultAsync(p => p.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderDto = new OrderDto()
            {
                Id = id,
                CustomerId = order.Id,
                OrderDate = order.OrderDate,
                OrderItemsDto = ConverItemToDto(order.OrderItems),
                OrderStatus = order.OrderStatus,
                ShippingAddressId = order.ShippingAddressId,
                TotalAmount = order.TotalAmount,
            };
            return Ok(orderDto);
        }

        /// <summary>
        /// Создание нового заказа
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PostAsync(OrderDto orderDto)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = orderDto.CustomerId,
                OrderStatus = OrderEnum.Active,
                OrderDate = orderDto.OrderDate,
                TotalAmount = orderDto.TotalAmount,
                ShippingAddressId = orderDto.ShippingAddressId
            };

            var orderItems = new List<OrderItem>();
            foreach (var orderItemDto in orderDto.OrderItemsDto)
            {
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = orderItemDto.ProductId,
                    Quantity = orderItemDto.Quantity,
                    Price = orderItemDto.Price
                };

                orderItems.Add(orderItem);
            }

            // Сохранение заказа в базе данных
            order.OrderItems = orderItems;

            await db.Orders.AddAsync(order);
            db.SaveChanges();

            return Ok(order.Id); // Возвращаем идентификатор нового заказа
        }

        [HttpPut("cancel")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            var order = await db.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatus = OrderEnum.Cancelled;
            await db.Orders.AddAsync(order);
            db.SaveChanges();

            return Ok(order.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var order = await db.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();
            return Ok("Deleted successfully");
        }

        private List<OrderItemsDto> ConverItemToDto(List<OrderItem> item)
        {
            if (item == null)
            {
                return null;
            }
            var orderItems = new List<OrderItemsDto>();
            foreach (var orderItem in item)
            {
                var orderItemDto = new OrderItemsDto
                {
                    Id = orderItem.Id,
                    OrderId = orderItem.OrderId,
                    ProductId = orderItem.ProductId,
                    Quantity = orderItem.Quantity,
                    Price = orderItem.Price
                };

                orderItems.Add(orderItemDto);
            }

            return orderItems;
        }
    }
}
