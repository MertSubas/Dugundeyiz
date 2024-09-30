using Dugundeyiz.Models;
using Dugundeyiz.ViewModels;
using Dugundeyiz.Context;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Dugundeyiz.Entity;
using System.ComponentModel.DataAnnotations;

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

        [Route("AddCategory")]
        public IActionResult AddCategory([FromForm] Newcategory newCategory)
        {
            var fileExtension = Path.GetExtension(newCategory.image.FileName);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            if (allowedExtensions.Contains(fileExtension.ToLower()))
            {
                // Resmi kaydedeceğimiz klasör
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CategoryImages");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Dosya adını oluştur
                var uniqueFileName = $"category_0_{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    newCategory.image.CopyTo(fileStream);
                }
                var maxSortingValue = _context.Categories.Max(c => c.Sorting);
                var newSorting = maxSortingValue + 1;
                Category addCategory = new Category();
                addCategory.MainCategoryID = null;
                addCategory.Sorting = newSorting;
                addCategory.Image = $"/CategoryImages/{uniqueFileName}";
                addCategory.CategoryName = newCategory.categoryName;
                _context.Categories.Add(addCategory);

                _context.SaveChanges();

                return View();

            }
            return View();
        }

        [Route("DeleteCategory")]
        public IActionResult DeleteCategory(int categoryId)
        {
            var category = _context.Categories.Where(x => x.CategoryID == categoryId).FirstOrDefault();
            _context.Remove(category);
            _context.SaveChanges();

            return View();
        }       
        [Route("AddSubCategory")]
        public IActionResult AddSubCategory(int mainCategoryId, string subcategoryName)
        {
            var category = _context.Categories.Where(x => x.CategoryID == mainCategoryId && x.Deleted!=true).FirstOrDefault();
            if (category != null)
            {
                Category subCategory = new Category();
                subCategory.MainCategoryID = mainCategoryId;
                subCategory.CategoryName=subcategoryName;
                _context.Categories.Add(subCategory);
                _context.SaveChanges();

            }

            return View();
        }


        [Route("UpdateCategory")]
        public IActionResult UpdateCategory([FromForm] CategoryFormUpdate updatedCategory)
        {
            try
            {
                var category = _context.Categories.Where(x => x.CategoryID == updatedCategory.CategoryID).FirstOrDefault();
                if (category != null)
                {
                    if (updatedCategory.uploadedImage != null && updatedCategory.Image != "")
                    {
                        var fileExtension = Path.GetExtension(updatedCategory.uploadedImage.FileName);
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                        if (allowedExtensions.Contains(fileExtension.ToLower()))
                        {
                            // Resmi kaydedeceğimiz klasör
                            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CategoryImages");
                            if (!Directory.Exists(uploadsFolder))
                            {
                                Directory.CreateDirectory(uploadsFolder);
                            }

                            // Dosya adını oluştur
                            var uniqueFileName = $"category_{updatedCategory.CategoryID}_{Guid.NewGuid()}{fileExtension}";
                            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                updatedCategory.uploadedImage.CopyTo(fileStream);
                            }

                            // Veritabanına kaydedilecek dosya yolu
                            category.Image = $"/CategoryImages/{uniqueFileName}";
                            category.CategoryName=updatedCategory.CategoryName;
                            _context.SaveChanges();

                            return View();

                        }
                        return View();

                    }
                    else
                    {
                        category.CategoryName = updatedCategory.CategoryName;
                        _context.SaveChanges();
                        return View();

                    }

                }
                else
                {
                    return View();

                }
            }
            catch (Exception e)
            {
                return View();

            }


        }



        [Route("UpdateCategorySorting")]
        [HttpPost]
        public JsonResult UpdateCategorySorting([FromBody] SortingUpdateModel sortingUpdateModel)
        {
            try
            {
                var draggedCategoryId = sortingUpdateModel.DraggedCategoryId;
                var newSorting = sortingUpdateModel.NewSorting;

                var draggedCategory = _context.Categories.Find(draggedCategoryId);
                if (draggedCategory == null)
                    return null; // Kategori bulunamadı

                var oldSorting = draggedCategory.Sorting;

                // Eğer yeni sıralama eski sıralamadan büyükse, aradaki kategorilerin sıralamasını azalt
                if (newSorting > oldSorting)
                {
                    var categoriesToUpdate = _context.Categories
                        .Where(c => c.Sorting > oldSorting && c.Sorting  <= newSorting && c.Deleted != true && c.MainCategoryID==null)
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
                        .Where(c => c.Sorting < oldSorting && c.Sorting >= newSorting && c.Deleted != true && c.MainCategoryID == null)
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
        public IActionResult ManagmentCategorySortingList()
        {
            var categories = _context.Categories.Where(x=>x.MainCategoryID==null && x.Deleted !=true).OrderBy(c => c.Sorting).ToList();

            return View(categories);
        }


        [Route("KategoriYonetimi")]
        public IActionResult ManagmentCategoryList()
        {
            //List<CategoryManamentViewModel> categoryManamentViewModel = new List<CategoryManamentViewModel>();
            //categoryManamentViewModel = _context.Categories
            //    .Where(x => x.MainCategoryID == null)
            //    .OrderBy(c => c.Sorting)
            //    .Select(c => new CategoryManamentViewModel
            //    {
            //        CategoryID = c.CategoryID,
            //        MainCategoryID = c.MainCategoryID,
            //        CategoryName = c.CategoryName,
            //        Image = c.Image,
            //        Deleted = c.Deleted,
            //        Sorting = c.Sorting,
            //        // Alt kategorileri Subcategory listesine ekle
            //        Subcategory = _context.Categories
            //            .Where(sub => sub.MainCategoryID == c.CategoryID)
            //            .OrderBy(sub => sub.Sorting)
            //            .ToList() // Alt kategorileri listele
            //    })
            //    .ToList();

            //return View(categoryManamentViewModel);


            var categories = _context.Categories.OrderBy(c => c.Sorting).ToList();

            return View(categories);
        }

    }
}
