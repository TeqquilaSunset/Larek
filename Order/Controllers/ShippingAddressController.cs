using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductOrder.Models;

namespace ProductOrder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShippingAddressController : Controller
    {
        ApplicationContext db = new ApplicationContext();

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var shippingaddress = await db.ShippingAddresses.ToListAsync();

            if(!shippingaddress.Any())
            {
                return NotFound();
            }

            var shippingAddressDto = shippingaddress.Select(p => new ShippingAddressDto
            {
                Id = p.Id,
                City= p.City,
                State= p.State,
                PostalCode= p.PostalCode,
                Country= p.Country,
                Street= p.Street,
            });

            return Ok(shippingAddressDto);
        }
    }
}
