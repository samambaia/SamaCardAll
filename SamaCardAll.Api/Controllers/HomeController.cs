using Microsoft.AspNetCore.Mvc;

namespace SamaCardAll.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Success!");
        }
    }
}
