using Dugundeyiz.Context;
using Dugundeyiz.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dugundeyiz.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DugundeyizContext _context;


        public CategoryController(ILogger<HomeController> logger, DugundeyizContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            var categories = _context.Categories.Where(x => x.Deleted != true && x.MainCategoryID==null).ToList();
            CategoryListPageViewModel categoryListPageViewModel = new CategoryListPageViewModel();
            categoryListPageViewModel.Categories = categories;
            return View(categoryListPageViewModel);
        }

        [Route("Category/Product/{categoryName}")]
        public IActionResult Product(string categoryName)
        {
            // id parametresi ile istediğiniz işlemleri yapabilirsiniz
            var category = _context.Categories.Where(x => x.CategoryName == categoryName && x.Deleted != true).FirstOrDefault();
            return View();
        }
    }
}
