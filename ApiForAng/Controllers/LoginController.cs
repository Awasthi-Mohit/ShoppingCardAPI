using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiForAng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public LoginController(IHttpClientFactory _httpClientFactory)
        {
            _httpClientFactory = _httpClientFacto0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000ry;
        }
        [HttpPost]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }
    }
}
