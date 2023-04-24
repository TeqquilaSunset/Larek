using ProductOrder.Models;

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
                Order order = new Order()
                {
                    CustomerName= "test",
                    CustomerAdress = "",
                    CustomerEmail = "",
                    CustomerPhone= "123",
                    CreatedDate= DateTime.Now,
                    Quantity=1,
                    ProductId = Guid.NewGuid()
                };

                db.Add(order);
                db.SaveChanges();
            
            }
            app.Run();
        }
    }
}