using Microsoft.AspNetCore.Mvc;

namespace Dugundeyiz.Controllers
{
    [Route("/Hakkimizda")]
    public class InformationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
