using Catalog;
using Catalog.Models;

namespace Catalog
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

            Category category1 = new Category()
            {
                Id = Guid.NewGuid(),
                Name = "�����������",
                Description = "����������� ������� � ������ �����"
            };

            Category category2 = new Category()
            {
                Id = Guid.NewGuid(),
                Name = "������� �������",
                Description = "������� ��� ������� ����"
            };

            Brand brand = new Brand()
            {
                Id = Guid.NewGuid(),
                Name = "SAMSUNG",
                Description = "������� ���, �� �� ��",
            };


            Product newProduct = new Product
            {
                Id = Guid.NewGuid(), // ��������� ������ ����������� �������������� ���� Guid
                Name = "New Product",
                Description = "A new product",
                Price = 9.99M,
                BrandId = brand.Id, // ��������� �������������� ���������� ������� Brand
                CategoryId = category1.Id,
            };
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Brands.Add(brand);
                db.Categories.AddRange(category1, category2);
                db.Products.Add(newProduct);
                db.SaveChanges();
            }

            app.Run();

        }
    }
}