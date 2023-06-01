using Microsoft.AspNetCore.Mvc;
using ProductOrder.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ProductOrder.Models.Dtos;
using ProductOrder.Enum;
using static NuGet.Packaging.PackagingConstants;
using Newtonsoft.Json;
using System.Text;

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
        public async Task<ActionResult> GetAll()
        {
            var orders = await db.Orders.Include(p => p.OrderItems).ToListAsync();

            if (!orders.Any())
            {
                return NotFound();
            }

            var ordersDto = orders.Select(p => new OrderDto
            {
                Id = p.Id,
                CustomerId = p.CustomerId,
                OrderDate = p.OrderDate,
                OrderItemsDto = ConvertItemToDto(p.OrderItems),
                Delivery = p.Delivery,
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
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                OrderItemsDto = ConvertItemToDto(order.OrderItems),
                Delivery = order.Delivery,
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
            if (orderDto.OrderItemsDto == null)
            {
                BadRequest("Null Items");
            }
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = orderDto.CustomerId,
                OrderStatus = OrderEnum.Active,
                OrderDate = DateTime.Now,
                Delivery = orderDto.Delivery,
                TotalAmount = orderDto.TotalAmount,
                ShippingAddressId = orderDto.ShippingAddressId
            };

            decimal totalAmount = 0;
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
                totalAmount += orderItemDto.Price * orderItemDto.Quantity;
                orderItems.Add(orderItem);
            }

            order.OrderItems = orderItems;
            order.TotalAmount = totalAmount;

            if (order.Delivery == true)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var requestBody = new CollectedOrders()
                        {
                            Id = Guid.NewGuid(),
                            NameDeliverer = "Иван Панаев",
                            OrderId = order.Id
                        };

                        var requestData = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                        string apiUrl = "http://localhost:5064";
                        string requestUrl = $"{apiUrl}/Delivery";
                        HttpResponseMessage response = await client.PostAsync(requestUrl, requestData);

                        string responseBody = await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }

            db.Add(order);
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

        private List<OrderItemsDto> ConvertItemToDto(List<OrderItem> item)
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
