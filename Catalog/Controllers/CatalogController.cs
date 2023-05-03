using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using Catalog.Models;
using System;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : Controller
    {

        ApplicationContext db = new ApplicationContext();

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(Guid id)
        {
            //var products = await db.Products.FindAsync(id);
            var products = await db.Products.Include(p => p.Brand).Include(p => p.Category).ToListAsync();

            var productDto = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                BrandId = p.Brand.Id,
                CategoryId = p.Category.Id,
                //BrandName = p.Brand.Name,
                //CategoryName = p.Category.Name
            });

            if (products == null)
            {
                return NotFound();
            }

            return Ok(productDto);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            var products = await db.Products.Include(p => p.Brand).Include(p => p.Category).ToListAsync();

            if (!products.Any())
            {
                return NoContent();
            }

            var productDto = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                BrandId = p.Brand.Id,
                CategoryId = p.Category.Id,
                //BrandName = p.Brand.Name,
                //CategoryName = p.Category.Name
            }).ToList();


            return Ok(productDto);
        }

        [HttpPost]
        public async Task<ActionResult> Post(ProductDto productDto)
        {
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                BrandId = productDto.BrandId,
                CategoryId = productDto.CategoryId
            };
            productDto.Id = product.Id;

            await db.Products.AddAsync(product);
            await db.SaveChangesAsync();

            return Ok(productDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(Guid id, ProductDto productDto)
        {
            var product = new Product()
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                BrandId = productDto.BrandId,
                CategoryId = productDto.CategoryId
            };

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok(productDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var products = db.Products.Find(id);

            if (products == null)
            {
                return NotFound();
            }
            db.Products.Remove(products);
            await db.SaveChangesAsync();

            return Ok();
        }

        private bool ProductsExists(Guid id)
        {
            return db.Products.Any(e => e.Id == id);
        }
    }
}
