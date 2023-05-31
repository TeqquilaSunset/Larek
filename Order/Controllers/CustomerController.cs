using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductOrder.Models;

namespace ProductOrder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        ApplicationContext db = new ApplicationContext();

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomer(Guid id)
        {
            var customers = await db.Customers.FindAsync(id);
            if(customers == null)
            {
                return NotFound();
            }
            return Ok(customers);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var customers = await db.Customers.ToListAsync();

            if(customers == null)
            {
                return NotFound();
            }

            var customersDto = customers.Select(p => new CustomerDto
            {
                Id= p.Id,
                Email= p.Email,
                FirstName= p.FirstName,
                LastName= p.LastName,
                PhoneNumber = p.PhoneNumber
            });

            return Ok(customersDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddBrand(CustomerDto customerDto)
        {

            var customer = new Customer()
            {
                Id = Guid.NewGuid(),
                Email= customerDto.Email,
                FirstName= customerDto.FirstName,
                LastName= customerDto.LastName,
                PhoneNumber= customerDto.PhoneNumber,

            };

            await db.Customers.AddAsync(customer);
            await db.SaveChangesAsync();

            customerDto.Id = customer.Id;
            return Ok(customerDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            var customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            await db.SaveChangesAsync();
            return Ok("Removed successfully");
        }
    }
}
