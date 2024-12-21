using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dugundeyiz.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin")] // Sadece Admin rolüne sahip kullanıcılar erişebilir
    public class HomeController : Controller
    {
        [HttpGet("/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return Redirect("https://dugundeyiz.com.tr"); // Ana sayfaya yönlendirme
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
