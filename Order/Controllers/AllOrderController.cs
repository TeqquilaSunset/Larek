using Microsoft.AspNetCore.Mvc;

namespace Order.Controllers
{
    public class AllOrderController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
