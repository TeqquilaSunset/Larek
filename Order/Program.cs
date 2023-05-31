using Microsoft.AspNetCore.Mvc;
using ProductOrder.Models;
using ProductOrder.Models.Dtos;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ProductOrder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMvc();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action}/{id?}");

            using(ApplicationContext db = new ApplicationContext()) 
            {
                var shippingAddress = new ShippingAddress()
                {
                    Id = Guid.NewGuid(),
                    Street = "Пастера",
                    City = "Томск",
                    State= "Томская область",
                    PostalCode = "36014",
                    Country = "Россия"
                };


                var customer = new Customer()
                {
                    Id = Guid.NewGuid(),
                    FirstName= "test",
                    LastName= "test",
                    Email= "test",
                    PhoneNumber= "test",
                };

                var order = new Order()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer.Id,
                    OrderDate = DateTime.Now,
                    OrderStatus = Enum.OrderEnum.Cancelled,
                    TotalAmount = 0,
                    OrderItems = new List<OrderItem>() { },
                    ShippingAddressId = shippingAddress.Id,
                };

                var orderItem = new OrderItem()
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = Guid.NewGuid(),
                    Quantity= 1,
                    Price = 1200,
                };

                var orderItem2 = new OrderItem()
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = Guid.NewGuid(),
                    Quantity = 3,
                    Price = 1340,
                };

                order.OrderItems.Add(orderItem);
                order.OrderItems.Add(orderItem2);

                //order.TotalAmount = orderItem.Price * orderItem.Quantity + orderItem2.Quantity * orderItem2.Price;
                //db.AddRange(shippingAdress, product1, customer, order, orderItem);
                //db.AddRange(shippingAdress, customer, order, orderItem);
                db.AddRange(shippingAddress, customer);
                db.SaveChanges();
                db.AddRange(order);
                db.SaveChanges();

            }
            app.Run();
        }
    }
}