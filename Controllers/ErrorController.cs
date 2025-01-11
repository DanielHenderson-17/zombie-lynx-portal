using Microsoft.AspNetCore.Mvc;

namespace ZombieLynxPortal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult HandleError([FromQuery] string message)
        {
            return Content($"Error: {message}");
        }
    }
}
