using Microsoft.AspNetCore.Mvc;

namespace SamaCardAll.Api.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("/api/Spend");
        }
    }
}
