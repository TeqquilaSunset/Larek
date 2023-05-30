using Microsoft.AspNetCore.Mvc;
using ProductOrder.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ProductOrder.Models.Dtos;
using ProductOrder.Enum;

namespace ProductOrder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        ApplicationContext db = new ApplicationContext();

        //[HttpGet]
        //public async Task<ActionResult> GetAllOrders()
        //{
        //    var orders = await db.Orders.ToListAsync();
        //    return Ok(orders);
        //}

        //[HttpGet]
        //public ActionResult Get()
        //{
        //    var orders = db.Orders.ToList();

        //    if (!orders.Any())
        //    {
        //        return NotFound();
        //    }
        //    return Ok(orders);
        //}

        [HttpGet]
        public ActionResult Get()
        {
            var customers = db.ShippingAddresses.ToList();

            if (!customers.Any())
            {
                return NotFound();
            }
            return Ok(customers);
        }

        //[HttpGet]
        //public async Task<ActionResult> GetOrderItem()
        //{
        //    var orderItems = await db.OrderItems.ToListAsync();
        //    return Ok(orderItems);
        //}

        [HttpGet("{id}")]
        public ActionResult Get(Guid id)
        {
            var order = db.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
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

            db.Orders.Add(order);
            db.SaveChanges();

            return Ok(order.Id); // Возвращаем идентификатор нового заказа
        }

        [HttpPut("cancel")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            var order = await db.Orders.FindAsync(id);

            if(order == null)
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
    }
}
