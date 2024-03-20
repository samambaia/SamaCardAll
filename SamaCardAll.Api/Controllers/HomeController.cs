using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SamaCardAll.Api;

namespace SamaCardAll.Api.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("/api/Spend/ping");
        }
    }
}
