using Dugundeyiz.Models;
using Dugundeyiz.ViewModels;
using Dugundeyiz.Context;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dugundeyiz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DugundeyizContext _context;


        public HomeController(ILogger<HomeController> logger, DugundeyizContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.Where(x => x.Deleted != true && x.MainCategoryID==null).OrderBy(x=>x.Sorting).ToList();
            var subcategories = _context.Categories.Where(x => x.Deleted != true && x.MainCategoryID!=null).ToList();
            HomeViewModel HomePageData = new HomeViewModel() { };
            HomePageData.Kategoriler = categories;
            HomePageData.AltKategoriler = subcategories;

            return View(HomePageData);
        }
        public IActionResult Checking()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
