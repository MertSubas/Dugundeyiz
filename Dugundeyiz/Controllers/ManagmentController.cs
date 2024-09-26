using Dugundeyiz.Models;
using Dugundeyiz.ViewModels;
using Dugundeyiz.Context;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dugundeyiz.Controllers
{
    [Route("/YonetimPaneli")]
    public class ManagmentController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DugundeyizContext _context;


        public ManagmentController(ILogger<HomeController> logger, DugundeyizContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddCategory(CategoryViewModels category)
        {
            return View();
        }
        public IActionResult DeleteCategory(int categoryId)
        {
            return View();
        }

        public class SortingUpdateModel
        {
            public int DraggedCategoryId { get; set; }
            public int NewSorting { get; set; }
        }

        [Route("UpdateCategorySorting")]
        [HttpPost]
        public JsonResult UpdateCategorySorting([FromBody] SortingUpdateModel sortingUpdateModel)
        {
            try
            {
                var draggedCategoryId= sortingUpdateModel.DraggedCategoryId;
                var newSorting = sortingUpdateModel.NewSorting;

                var draggedCategory = _context.Categories.Find(draggedCategoryId);
                if (draggedCategory == null)
                    return null; // Kategori bulunamadı

                var oldSorting = draggedCategory.Sorting;

                // Eğer yeni sıralama eski sıralamadan büyükse, aradaki kategorilerin sıralamasını azalt
                if (newSorting > oldSorting)
                {
                    var categoriesToUpdate = _context.Categories
                        .Where(c => c.Sorting > oldSorting && c.Sorting <= newSorting && c.Deleted != true)
                        .OrderBy(c => c.Sorting)
                        .ToList();

                    foreach (var category in categoriesToUpdate)
                    {
                        category.Sorting -= 1;
                    }
                }
                // Eğer yeni sıralama eski sıralamadan küçükse, aradaki kategorilerin sıralamasını artır
                else if (newSorting < oldSorting)
                {
                    var categoriesToUpdate = _context.Categories
                        .Where(c => c.Sorting < oldSorting && c.Sorting >= newSorting && c.Deleted != true)
                        .OrderByDescending(c => c.Sorting)
                        .ToList();

                    foreach (var category in categoriesToUpdate)
                    {
                        category.Sorting += 1;
                    }
                }

                // Taşınan kategorinin yeni sıralama değeri güncellenir
                draggedCategory.Sorting = newSorting;

                // Değişiklikleri kaydet
                _context.SaveChanges();

                return new JsonResult(new { status = "Ok" });

            }
            catch (Exception e)
            {
                return new JsonResult(new { status = e.ToString() });
            }


        }

        [Route("KategoriSiralamasi")]
        public IActionResult ManagmentCategoryList()
        {
            var categories = _context.Categories.OrderBy(c => c.Sorting).ToList();

            return View(categories);
        }

    }
}
