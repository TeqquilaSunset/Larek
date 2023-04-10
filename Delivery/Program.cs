using Delivery.Models;

namespace Delivery
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMvc();
            var app = builder.Build();

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