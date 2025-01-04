using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Dugundeyiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecaptchaController : ControllerBase
    {
        private const string RecaptchaSecret = "6Lfy4aQqAAAAAAjNvZ9lP_4yeF4K31do0dv8Zs7a"; // Gizli anahtar

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] string token)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={RecaptchaSecret}&response={token}"
            );

            var result = Newtonsoft.Json.Linq.JObject.Parse(response);
            bool success = result["success"].Value<bool>();

            if (success)
                return Ok(new { success = true });
            else
                return BadRequest(new { success = false, errors = result["error-codes"] });
        }
    }
}
