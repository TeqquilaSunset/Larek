using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Catalog.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategorysController : Controller
    {
        ApplicationContext db = new ApplicationContext();

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategorys(Guid id)
        {
            var categorys = await db.Categories.FindAsync(id);
            if(categorys == null)
            {
                return NoContent();
            }

            var categorysDto = new CategoryDto()
            {
                Id = categorys.Id,
                Name = categorys.Name,
                Description = categorys.Description
            };

            return Ok(categorys);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllCategorys()
        {
            var categorys = await db.Categories.ToListAsync();

            if (!categorys.Any())
            {
                return NotFound();
            }

            var categorysDto = categorys.Select(b => new CategoryDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description
            });

            return Ok(categorysDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddCategory(CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest();
            }

            var newCategory = new Category()
            {
                Id = Guid.NewGuid(),
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            categoryDto.Id= newCategory.Id;

            await db.Categories.AddAsync(newCategory);
            await db.SaveChangesAsync();

            return Ok(categoryDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return Ok("Removed successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(Guid id, CategoryDto categoryDto)
        {

            var category = new Category()
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            if (id != category.Id)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok(categoryDto);
        }
    }
}
