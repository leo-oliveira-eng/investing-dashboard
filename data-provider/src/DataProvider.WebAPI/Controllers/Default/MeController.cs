using Microsoft.AspNetCore.Mvc;

namespace DataProvider.WebAPI.Controllers.Default
{
    [ApiController]
    [Route("[controller]")]
    public class MeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetInformation()
            => Ok(new { name = "Investing Data Provider", version = "0.1.0" });
    }
}
