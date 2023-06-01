using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Catalog;
using Catalog.Models;
using Catalog.Models.Dtos;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrandsController : Controller
    {
        ApplicationContext db = new ApplicationContext();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrand(Guid id)
        {
            var brand = await db.Brands.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }
            var brandsDto = new BrandDto()
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description
            };

            return Ok(brandsDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await db.Brands.ToListAsync();

            if (brands == null)
            {
                return NoContent();
            }
            var brandsDto = brands.Select(b => new BrandDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description
            });

            return Ok(brandsDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddBrand(BrandDto newBrand)
        {

            var brand = new Brand()
            {
                Id = Guid.NewGuid(),
                Name = newBrand.Name,
                Description = newBrand.Description
            };

            await db.Brands.AddAsync(brand);
            await db.SaveChangesAsync();

            newBrand.Id = brand.Id;
            return Ok(newBrand);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            var brand = await db.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            db.Brands.Remove(brand);
            await db.SaveChangesAsync();
            return Ok("Removed successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(Guid id, BrandDto updateBrand)
        {
            if (id != updateBrand.Id)
            {
                return BadRequest();
            }

            var brand = new Brand()
            {
                Id = id,
                Name = updateBrand.Name,
                Description = updateBrand.Description
            };

            db.Entry(brand).State = EntityState.Modified;

            if (brand == null)
            {
                return NotFound();
            }

            await db.SaveChangesAsync();
            return Ok(updateBrand);
        }

    }
}
