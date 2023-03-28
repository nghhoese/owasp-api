using BusinesLogic.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VideoSharing.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        // GET: api/<AuthController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(_jwtService.GetJwtToken());
        }
    }
}