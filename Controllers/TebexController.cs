using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZombieLynxPortal.Services;

namespace ZombieLynxPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TebexController : ControllerBase
    {
        private readonly ITebexApiService _tebexApiService;

        public TebexController(ITebexApiService tebexApiService)
        {
            _tebexApiService = tebexApiService;
        }

        [HttpGet("packages")]
        public async Task<IActionResult> GetPackages()
        {
            var result = await _tebexApiService.GetAllPackagesAsync();
            return Ok(result);
        }

        // [HttpPost("basket")]
        // public async Task<IActionResult> CreateBasket()
        // {
        //     var result = await _tebexApiService.CreateBasketAsync();
        //     return Ok(result);
        // }
    }
}
