using Microsoft.AspNetCore.Mvc;

namespace RealEstateManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API works");
        }
    }
}