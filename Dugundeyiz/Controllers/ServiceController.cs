using Dugundeyiz.Context;
using Microsoft.AspNetCore.Mvc;
using Dugundeyiz.ViewModels;

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
            var categoryId = _context.Categories.Where(x => x.CategoryName == categoryName && x.Deleted != true).FirstOrDefault().CategoryID;
            var services= _context.Services.Where(x=>x.CategoryID== categoryId && x.Deleted!= true).OrderByDescending(x=>x.Sorting).ToList();
            ServiceListPageViewModel serviceListPageViewModel = new ServiceListPageViewModel();
            serviceListPageViewModel.Servisler= services;
            return View();
        }
    }
}
