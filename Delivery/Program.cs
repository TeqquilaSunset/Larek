using Delivery.Models;

namespace Delivery
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

            ApplicationContext db = new();

            CollectedOrders collectedOrders = new CollectedOrders()
            {
                NameDeliverer = "Ivan",
                CollectedDate = DateTime.Now,
                IsCollected = false,
                OrderId = Guid.NewGuid(),
            };

            db.CollectedOrders.Add(collectedOrders);
            db.SaveChanges();

            app.Run();
        }
    }
}