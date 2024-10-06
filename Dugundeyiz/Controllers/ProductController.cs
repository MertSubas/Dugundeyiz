using Dugundeyiz.Context;
using Microsoft.AspNetCore.Mvc;
using Dugundeyiz.ViewModels;

namespace Dugundeyiz.Controllers
{
    [Route("/Hizmetler")]
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DugundeyizContext _context;


        public ProductController(ILogger<HomeController> logger, DugundeyizContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("{categoryName}")]
        public IActionResult Index(string categoryName)
        {
            var categoryId = _context.Categories.Where(x => x.CategoryName == categoryName && x.Deleted != true).FirstOrDefault().CategoryID;
            var services = _context.Products.Where(x => x.CategoryID == categoryId && x.Deleted != true).OrderByDescending(x => x.Sorting).ToList();
            ProductListPageViewModel productListPageViewModel = new ProductListPageViewModel();
            productListPageViewModel.Products = services;

            ///kaldırılacak
            var addTo = services.ElementAt(0);
            for (int i = 0; i <= 22; i++)
            {
                services.Add(addTo);

            }



            return View(productListPageViewModel);
        }

        [Route("samplePage/{categoryName}")]
        public IActionResult SamplePage(string categoryName)
        {
            var categoryId = _context.Categories.Where(x => x.CategoryName == categoryName && x.Deleted != true).FirstOrDefault().CategoryID;
            var services = _context.Products.Where(x => x.CategoryID == categoryId && x.Deleted != true).OrderByDescending(x => x.Sorting).ToList();
            ProductListPageViewModel productListPageViewModel = new ProductListPageViewModel();
            productListPageViewModel.Products = services;

            ///kaldırılacak
            var addTo = services.ElementAt(0);
            for (int i = 0; i <= 22; i++)
            {
                services.Add(addTo);

            }



            return View(productListPageViewModel);
        }


        public IActionResult AddProduct(AddProductPageViewModel productToAdd)
        {

            return View(); 

        }
    }
}
