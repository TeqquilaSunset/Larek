using Delivery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryController : Controller
    {
        ApplicationContext db = new();

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(Guid id)
        {
            var collectOrders = await db.CollectedOrders.FindAsync(id);
            if(collectOrders == null)
            {
                NotFound();
            }
            return Ok(collectOrders);
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var collectOrders = await db.CollectedOrders.ToListAsync();
            return Ok(collectOrders);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(CollectedOrders collectedOrders)
        {
            await db.CollectedOrders.AddAsync(collectedOrders);

            await db.SaveChangesAsync();
            return Ok(collectedOrders);
        }

        //[HttpPut]
        //public async Task<ActionResult> 

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletAsync(Guid id)
        {
            var colletedOrder = await db.CollectedOrders.FindAsync(id);

            if(colletedOrder == null)
            {
                return NotFound();
            }

            db.CollectedOrders.Remove(colletedOrder);
            await db.SaveChangesAsync();
            return Ok("Deleted successfully");
        }
    }
}
