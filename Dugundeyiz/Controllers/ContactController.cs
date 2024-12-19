using Microsoft.AspNetCore.Mvc;

namespace Dugundeyiz.Controllers
{
    [Route("/Iletisim")]
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
