using Dugundeyiz.Context;
using Microsoft.AspNetCore.Mvc;

namespace Dugundeyiz.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DugundeyizContext _context;


        public ServiceController(ILogger<HomeController> logger, DugundeyizContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("Hizmetler/{categoryName}")]
        public IActionResult Index(string categoryName)
        {
            var category = _context.Categories.Where(x => x.CategoryName == categoryName && x.Deleted != true).FirstOrDefault();
            return View();
        }
    }
}
