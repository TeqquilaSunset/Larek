using Microsoft.AspNetCore.Mvc;
using ProductOrder.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ProductOrder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        ApplicationContext db = new ApplicationContext();

        [HttpGet("{id}")]
        public ActionResult Get(Guid id)
        {
            var orders = db.Orders.Find(id);

            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        [HttpGet]
        public ActionResult Get()
        {
            var orders = db.Orders.ToList();

            if (!orders.Any())
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Order order)
        {
            order.CreatedDate = DateTime.Now;
            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();

            return Ok(order);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var order = await db.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();
            return Ok("Deleted successfully");
        }
    }
}
