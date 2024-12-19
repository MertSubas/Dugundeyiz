using Dugundeyiz.Models;
using Dugundeyiz.ViewModels;
using Dugundeyiz.Context;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Dugundeyiz.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Dugundeyiz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DugundeyizContext _context;
        private readonly UserManager<AppUser> _userManager;


        public HomeController(ILogger<HomeController> logger, DugundeyizContext context, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;

        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {

            var categories = _context.Categories.Where(x => x.Deleted != true && (x.MainCategoryID==null || x.MainCategoryID == 0)).OrderBy(x=>x.Sorting).ToList();
            var subcategories = _context.Categories.Where(x => x.Deleted != true && x.MainCategoryID != 0 && x.MainCategoryID!=null).ToList();
            HomeViewModel HomePageData = new HomeViewModel() { };
            HomePageData.Kategoriler = categories;
            HomePageData.AltKategoriler = subcategories;

            //var user = await _userManager.GetUserAsync(User);

            //if (user != null)
            //{
            //    user.
            //    var userRole=_context.Users.Where(x=>x.)

            //}
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
