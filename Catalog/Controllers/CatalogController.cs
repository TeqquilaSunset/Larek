using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using Catalog.Models;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : Controller
    {

        ApplicationContext db = new ApplicationContext();

        [HttpGet("{id}")]
        public ActionResult Get(Guid id)
        {
            var products = db.Products.Find(id);

            if (products == null)
            {
                return NotFound();
            }

            //var product = new Product()
            //{
            //    Name = productDto.Name,
            //    Description = productDto.Description,
            //    Price = productDto.Price,
            //    BrandId = productDto.BrandId,
            //    CategoryId = productDto.CategoryId
            //};

            return new JsonResult(products);
        }

        [HttpGet]
        public ActionResult Get()
        {
            var products = db.Products.Include(p => p.Brand).Include(p => p.Category).ToList();

            if (!products.Any())
            {
                return NotFound();
            }

            var productDto = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                BrandId = p.Brand.Id,
                CategoryId = p.Category.Id,
                BrandName = p.Brand.Name,
                CategoryName = p.Category.Name
            }).ToList();


            return new JsonResult(productDto);
        }

        [HttpPost]
        public ActionResult Post(ProductDto productDto)
        {
            var product = new Product()
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                BrandId = productDto.BrandId,
                CategoryId = productDto.CategoryId
            };

            db.Products.Add(product);
            db.SaveChanges();

            //var returnProductDto = new ProductReturnDto
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Description = product.Description,
            //    Price = product.Price,
            //    BrandName = product.Brand.Name,
            //    CategoryName = product.Category.Name
            //};

            var returnProduct = db.Products.Find(product.Id);
            return Ok(returnProduct);

            //if (product == null)
            //{
            //    return new JsonResult(BadRequest());
            //}

            //db.Products.Add(product);
            //db.SaveChanges();
            //return new JsonResult(product);
        }

        [HttpPut("{id}")]
        public JsonResult Put(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return new JsonResult(BadRequest());
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
                {
                    return new JsonResult(NotFound());
                }
                else
                {
                    throw;
                }
            }
            //return NoContent();
            //return await _context.TodoItem.FindAsync(id);
            return new JsonResult(product);
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(Guid id)
        {
            var products = db.Products.Find(id);

            if (products == null)
            {
                return new JsonResult(NotFound());
            }
            db.Products.Remove(products);
            db.SaveChanges();

            return new JsonResult(Ok());
        }

        private bool ProductsExists(Guid id)
        {
            return db.Products.Any(e => e.Id == id);
        }
    }
}
