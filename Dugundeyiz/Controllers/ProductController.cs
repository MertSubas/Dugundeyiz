using Dugundeyiz.Context;
using Microsoft.AspNetCore.Mvc;
using Dugundeyiz.ViewModels;
using Dugundeyiz.Entity;
using Microsoft.AspNetCore.Identity;
using Dugundeyiz.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using Dugundeyiz.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Dugundeyiz.Controllers
{
    [Route("/Hizmetler")]
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DugundeyizContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ProductService _productService;

        public ProductController(
            ILogger<HomeController> logger,
            DugundeyizContext context,
            UserManager<AppUser> userManager,
            ProductService productService)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _productService = productService;
        }
        [Route("")]
        public IActionResult Index()
        {
            var categories = _context.Categories.Where(x => x.Deleted != true && x.MainCategoryID == null).ToList();
            CategoryListPageViewModel categoryListPageViewModel = new CategoryListPageViewModel();
            categoryListPageViewModel.Categories = categories;
            return View(categoryListPageViewModel);
        }

        [Route("2024/Kampanyalar")]
        public IActionResult Campaigns()
        {
            return View();

        }


        [Route("{categoryName}")]
        public IActionResult ProductList(string categoryName)
        {
            var category = _context.Categories.Where(x => x.CategoryName == categoryName && x.Deleted != true).FirstOrDefault();
            /*
             var productsWithCategory = from p in _context.Products
                                        where p.Deleted == false && p.Visibility != false
                                        join c in _context.Categories on p.CategoryID equals c.CategoryID
                                        join city in _context.Citys on p.City equals city.Id
                                        join town in _context.Towns on p.District equals town.ID
                                        select new ProductMap
                                        {
                                            ProductID = p.ProductID,
                                            CategoryID = p.CategoryID,
                                            Name = p.Name,
                                            Capacity = p.Capacity,
                                            CampaignPricePerPerson = p.CampaignPricePerPerson,
                                            PricePerPerson = p.PricePerPerson,
                                            Text = p.Text,
                                            City = city.CityName,
                                            District = town.TownName,
                                            Images = p.Images,
                                            OwnerUserID = p.OwnerUserID,
                                            AproveByAdmin = p.AproveByAdmin,
                                            Sorting = p.Sorting,
                                            Deleted = p.Deleted ?? false,
                                            Visibility = p.Visibility ?? false,
                                            CategoryName = c.CategoryName // Assuming the category name is in the "Name" property of the Category table
                                        };

             ///kaldırılacak
             ///



             var maxPricePerPerson = productsWithCategory.Max(x => x.PricePerPerson ?? 0);
             var maxCampaignPricePerPerson = productsWithCategory.Max(x => x.CampaignPricePerPerson ?? 0);

             var maxPrice = Math.Max(maxPricePerPerson, maxCampaignPricePerPerson);

             var minPricePerPerson = productsWithCategory
                 .Where(x => x.PricePerPerson != null && x.PricePerPerson > 0)
                 .Min(x => x.PricePerPerson.Value);

             var minCampaignPricePerPerson = productsWithCategory
                 .Where(x => x.CampaignPricePerPerson != null && x.CampaignPricePerPerson > 0)
                 .Min(x => x.CampaignPricePerPerson.Value);

             var minPrice = Math.Min(minPricePerPerson, minCampaignPricePerPerson);



             Console.WriteLine($"Max Price: {maxPrice}");
             Console.WriteLine($"Min Price: {minPrice}");
             */

            var productsWithCategory = _productService.GetProductsWithCategory(categoryName);


            ProductListPageViewModel productListPageViewModel = new ProductListPageViewModel();
            productListPageViewModel.Products = productsWithCategory.ToList(); ;
            productListPageViewModel.ManName = categoryName;
            productListPageViewModel.Category = category;


            if (productsWithCategory.Count() >= 2)
            {
                var (maxPrice, minPrice) = _productService.GetMinMaxPrices(productsWithCategory);
                productListPageViewModel.maxPrice = maxPrice;
                productListPageViewModel.minPrice = minPrice;
            }

            if (productListPageViewModel.Products.Count != 0)
            {
                var addTo = productListPageViewModel.Products.ElementAt(0);
                for (int i = 0; i <= 22; i++)
                {
                    productListPageViewModel.Products.Add(addTo);

                }
            }



            return View(productListPageViewModel);
        }

        [HttpPost]
        [Route("{categoryName}/filter")]
        public IActionResult Filter([FromBody] FilterModel filters)
        {
            var category = _context.Categories.Where(x => x.CategoryName == "Gelinlik" && x.Deleted != true).FirstOrDefault();

            /*
            var query = _context.Products
    .Where(p => p.Deleted == false && p.Visibility != false)
    .Join(_context.Categories,
          p => p.CategoryID,
          c => c.CategoryID,
          (p, c) => new { Product = p, Category = c })
    .Join(_context.Citys,
          pc => pc.Product.City,
          city => city.Id,
          (pc, city) => new { pc.Product, pc.Category, City = city })
    .Join(_context.Towns,
          pcCity => pcCity.Product.District,
          town => town.ID,
          (pcCity, town) => new { pcCity.Product, pcCity.Category, pcCity.City, Town = town });

            if (!string.IsNullOrEmpty(filters.City) && filters.City != "Şehir")
            {
                query = query.Where(x => x.City.CityName == filters.City);
            }

            if (!string.IsNullOrEmpty(filters.District) && filters.District != "İlçe")
            {
                query = query.Where(x => x.Town.TownName == filters.District);
            }

            if (filters.MaxPrice != null)
            {
                query = query.Where(x => x.Product.PricePerPerson <= filters.MaxPrice ||
                                         x.Product.CampaignPricePerPerson <= filters.MaxPrice);
            }

            if (filters.MinPrice != null)
            {
                query = query.Where(x => x.Product.PricePerPerson >= filters.MinPrice ||
                                         x.Product.CampaignPricePerPerson >= filters.MinPrice);
            }

            var productsWithCategory = query.Select(x => new ProductMap
            {
                ProductID = x.Product.ProductID,
                CategoryID = x.Category.CategoryID,
                Name = x.Product.Name,
                Capacity = x.Product.Capacity,
                PricePerPerson = x.Product.PricePerPerson,
                CampaignPricePerPerson = x.Product.CampaignPricePerPerson,
                Text = x.Product.Text,
                City = x.City.CityName,
                District = x.Town.TownName,
                Images = x.Product.Images,
                OwnerUserID = x.Product.OwnerUserID,
                AproveByAdmin = x.Product.AproveByAdmin,
                Sorting = x.Product.Sorting,
                Deleted = x.Product.Deleted ?? false,
                Visibility = x.Product.Visibility ?? false,
                CategoryName = x.Category.CategoryName
            }).ToList();


            */
            var maxPrice = 9999;
            var minPrice = 0;
            var filteredProducts = _productService.FilterProducts(filters);
            if (filteredProducts.Count >= 2)
            {
                (maxPrice, minPrice) = _productService.GetMinMaxPrices(filteredProducts.AsQueryable());
            }
            ProductListPageViewModel productListPageViewModel = new ProductListPageViewModel();
            productListPageViewModel.Products = filteredProducts;
            productListPageViewModel.ManName = "Gelinlik";
            productListPageViewModel.Category = category;
            productListPageViewModel.maxPrice = maxPrice;
            productListPageViewModel.minPrice = minPrice;

            ///kaldırılacak
            if (productListPageViewModel.Products.Count != 0)
            {
                var addTo = productListPageViewModel.Products.ElementAt(0);
                for (int i = 0; i <= 22; i++)
                {
                    productListPageViewModel.Products.Add(addTo);

                }
            }

            return PartialView("_ListPartial", productListPageViewModel);
        }

        [Route("samplePage/{categoryName}")]
        public IActionResult SamplePage(string categoryName)
        {
            var categoryId = _context.Categories.Where(x => x.CategoryName == categoryName && x.Deleted != true).FirstOrDefault().CategoryID;
            var services = _context.Products.Where(x => x.CategoryID == categoryId && x.Deleted != true).OrderByDescending(x => x.Sorting).ToList();
            ProductListPageViewModel productListPageViewModel = new ProductListPageViewModel();
            //productListPageViewModel.Products = services;
            productListPageViewModel.ManName = categoryName;
            ///kaldırılacak
            var addTo = services.ElementAt(0);
            for (int i = 0; i <= 22; i++)
            {
                services.Add(addTo);

            }



            return View(productListPageViewModel);
        }

        [Route("{categoryName}/{productName}/{productID}")]
        public IActionResult ProductPage(string categoryName, string productName, int productID)
        {
            var productData = _context.Products.Where(x => x.ProductID == productID).FirstOrDefault();
            if (productData != null)
            {
                List<string> PhotoList = new List<string>();
                PhotoList.Add("/CategoryImages/category_2_bcfb6725-970c-41a9-822d-2e9baf0221e1.png");
                PhotoList.Add("/CategoryImages/category_17_c110a6b0-4879-48ae-aa29-9488a1c6b463.png");
                PhotoList.Add("/CategoryImages/category_2_bcfb6725-970c-41a9-822d-2e9baf0221e1.png");
                PhotoList.Add("/CategoryImages/category_17_c110a6b0-4879-48ae-aa29-9488a1c6b463.png");
                PhotoList.Add("/CategoryImages/category_2_bcfb6725-970c-41a9-822d-2e9baf0221e1.png");
                PhotoList.Add("/CategoryImages/category_17_c110a6b0-4879-48ae-aa29-9488a1c6b463.png");
                PhotoList.Add("/CategoryImages/category_2_bcfb6725-970c-41a9-822d-2e9baf0221e1.png");
                PhotoList.Add("/CategoryImages/category_17_c110a6b0-4879-48ae-aa29-9488a1c6b463.png");
                PhotoList.Add("/CategoryImages/category_2_bcfb6725-970c-41a9-822d-2e9baf0221e1.png");
                PhotoList.Add("/CategoryImages/category_17_c110a6b0-4879-48ae-aa29-9488a1c6b463.png");
                PhotoList.Add("/CategoryImages/category_2_bcfb6725-970c-41a9-822d-2e9baf0221e1.png");
                ProductPageViewModel productToSend = new ProductPageViewModel();
                productToSend.Product = productData;
                productToSend.PhotoList = PhotoList;
                return View(productToSend);

            }

            return View();

        }

        [Route("/AddProduct")]
        public IActionResult AddProduct(AddProductPageViewModel productToAdd)
        {

            return View();

        }

        [Route("ilanlarim")]
        public async Task<IActionResult> MyProducts(int userId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return Ok();
            }
            if (userId != null && userId != 0)
            {
                if (userId != user.Id)
                {
                    return Ok();
                }

            }
            else
            {
                userId = user.Id;
            }
            var productsWithCategory = from p in _context.Products
                                       where p.Deleted == false && p.OwnerUserID == userId
                                       join c in _context.Categories on p.CategoryID equals c.CategoryID
                                       join city in _context.Citys on p.City equals city.Id
                                       join town in _context.Towns on p.District equals town.ID
                                       select new ProductMap
                                       {
                                           ProductID = p.ProductID,
                                           CategoryID = p.CategoryID,
                                           Name = p.Name,
                                           Capacity = p.Capacity,
                                           PricePerPerson = p.PricePerPerson,
                                           Text = p.Text,
                                           City = city.CityName,
                                           District = town.TownName,
                                           Images = p.Images,
                                           OwnerUserID = p.OwnerUserID,
                                           AproveByAdmin = p.AproveByAdmin,
                                           Sorting = p.Sorting,
                                           Deleted = p.Deleted ?? false,
                                           Visibility = p.Visibility ?? false,
                                           CategoryName = c.CategoryName // Assuming the category name is in the "Name" property of the Category table
                                       };

            ProductListByUser productListByUser = new ProductListByUser();
            productListByUser.Products = productsWithCategory.ToList();
            productListByUser.userID = userId;

            return View(productListByUser);

        }

        [Route("ilanlarim/raporlar/{ilanId}")]
        public async Task<IActionResult> AnalysisPage(int ilanId)
        {
            return View();
        }


        [Route("RaporPage")]
        [HttpPost]
        public IActionResult RaporPage(int ilanID, int userID)
        {
            // Your logic to generate the report
            return Ok("Izin verildi");
        }

        [HttpPost]
        public IActionResult ModifyProduct(int ilanID, int userID)
        {
            // Your logic to modify the product
            return Ok("Ürün başarıyla güncellendi");
        }

        [Route("RemoveProduct")]
        [HttpPost]
        public IActionResult RemoveProduct(int ilanID, int userID)
        {
            var productData = _context.Products.Where(x => x.OwnerUserID == userID && x.ProductID == ilanID).FirstOrDefault();
            productData.Deleted = true;
            _context.SaveChanges();
            return Ok("İlan başarıyla kaldırıldı");
        }


        [Route("VisiblytProduct")]
        [HttpPost]
        public IActionResult VisiblytProduct(int ilanID, int userID)
        {
            // Your logic to make the product visible
            var productData = _context.Products.Where(x => x.OwnerUserID == userID && x.ProductID == ilanID).FirstOrDefault();
            if (productData.Visibility == null || productData.Visibility == true)
            {
                productData.Visibility = false;
                _context.SaveChanges();
                return Ok("İlan gizli hale getirildi");

            }
            else
            {
                productData.Visibility = true;
                _context.SaveChanges();
                return Ok("İlan görünür hale getirildi");
            }


        }


        //public IActionResult ReplyToComment([FromBody] Comment newReply)
        //{
        //    if (newReply == null || newReply.ReplyToCommentID == null)
        //    {
        //        return BadRequest("Geçersiz veri.");
        //    }

        //    newReply.CreateDate = DateTime.Now;
        //    _context.Comments.Add(newReply);
        //    _context.SaveChanges();

        //    return Ok(new { success = true });
        //}

        [Route("ilanlarim/{ilanId}/yorumlar")]
        public async Task<IActionResult> ProductComment(int ilanId)
        {
            var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
            var productData = _context.Products.Where(x => x.OwnerUserID == loggedUser.Id && x.ProductID == ilanId).FirstOrDefault();

            var data = (from comment in _context.Comments
                        join user in _context.Users
                        on comment.UserID equals user.UserID
                        where comment.SendToProductID == ilanId
                              && (comment.Deleted == null || comment.Deleted == false)
                        select new CommentViewModel
                        {
                            CommentID = comment.ID,
                            SendToProductID = comment.SendToProductID,
                            Message = comment.Message,
                            Title = comment.Title,
                            Checking = comment.Checking,
                            CreateDate = comment.CreateDate,
                            ReplyToCommentID = comment.ReplyToCommentID,
                            UserName = user.Name,
                            UserSurname = user.Surname
                        }).ToList();

            ProductCommentPageViewModel dataForView = new ProductCommentPageViewModel()
            {
                Comments = data,
                CurrentUserID = loggedUser.Id,
                Ilan = productData
            };

            return View(dataForView);
        }

        [Route("ilanlarim/YeniIlan")]
        public async Task<IActionResult> ProductAdd()
        {
            return View();
        }
        [HttpPost]
        [Route("ilanlarim/AddProduct")]
        public async Task<IActionResult> AddProduct(AddProductViewModel product)
        {
            // Diğer verilerin kaydı yapılabilir (örneğin, veritabanına ekleme)
            // ...
            try
            {

                // Verileri işleme
                var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
                Product newProduct = new Product();
                newProduct.CategoryID = product.Category;
                newProduct.Name = product.Title;
                newProduct.PricePerPerson = product.Price;
                newProduct.Capacity = product.Capacity.ToString();
                newProduct.Text = product.Description;
                newProduct.Deleted = false;
                newProduct.Visibility = true;
                newProduct.AproveByAdmin = 1;
                newProduct.OwnerUserID = loggedUser.Id;
                var cityId = await _context.Citys.Where(x => x.CityName == product.City).Select(x => x.Id).FirstOrDefaultAsync();
                var TownID = await _context.Towns.Where(x => x.TownName == product.District).Select(x => x.ID).FirstOrDefaultAsync();
                newProduct.City = cityId;
                newProduct.District = TownID;
                if(product.DeliveryTime != null || product.DeliveryTime != "")
                {
                    newProduct.DeliveryTime=product.DeliveryTime;
                }
                if (product.Features != null )
                {
                    newProduct.Features = string.Join(",", product.Features);
                }
                if (product.FeaturesView != null)
                {
                    newProduct.Views = string.Join(",", product.FeaturesView);
                }
                // Video, Kapak Fotoğrafı ve Fotoğraflar sunucuda kaydedilebilir
                if (product.Video != null)
                {
                    // Video kaydetme işlemi
                    var videoPath = Path.Combine("wwwroot/uploads/videos", product.Video.FileName);
                    using (var stream = new FileStream(videoPath, FileMode.Create))
                    {
                        product.Video.CopyTo(stream);
                    }
                    newProduct.Video = $"/uploads/videos/{product.Video.FileName}";

                }

                if (product.CoverPhoto != null)
                {
                    // Kapak fotoğrafını kaydetme işlemi
                    var coverPath = Path.Combine("wwwroot/uploads/kapak", product.CoverPhoto.FileName);
                    using (var stream = new FileStream(coverPath, FileMode.Create))
                    {
                        product.CoverPhoto.CopyTo(stream);
                    }
                    newProduct.Images = $"/uploads/kapak/{product.CoverPhoto.FileName}";

                }

                if (product.Photos != null && product.Photos.Any())
                {
                    using (var transaction = await _context.Database.BeginTransactionAsync()) // Transaction başlatıyoruz
                    {
                        try
                        {
                            // Ürün kaydını veritabanına ekliyoruz
                            _context.Products.Add(newProduct);
                            await _context.SaveChangesAsync();

                            foreach (var photo in product.Photos)
                            {
                                // Fotoğrafları kaydetme işlemi
                                var photoPath = Path.Combine("wwwroot/uploads/photos", photo.FileName);
                                using (var stream = new FileStream(photoPath, FileMode.Create))
                                {
                                    photo.CopyTo(stream);
                                }

                                // Fotoğraf kaydını yapıyoruz
                                ProductPhoto photoEntity = new ProductPhoto
                                {
                                    ProductID = newProduct.ProductID, // Burada yeni oluşan product'ın ID'sini alıyoruz
                                    PhotoPath = $"/uploads/photos/{photo.FileName}"
                                };

                                _context.ProductPhotos.Add(photoEntity);
                            }

                            // Fotoğrafları kaydediyoruz
                            await _context.SaveChangesAsync();

                            // Transaction başarılıysa commit ediyoruz
                            await transaction.CommitAsync();

                            return Ok(new { Message = "İlan başarıyla eklendi." });
                        }
                        catch (Exception ex)
                        {
                            // Hata oluşursa işlemi geri alıyoruz
                            await transaction.RollbackAsync();
                            return Ok(new { Message = "İlan Teknik Bir Sorun Nedeniyle Eklenemedi." });
                        }
                    }
                }
                else
                {
                    // Fotoğraf yoksa sadece ürünü ekliyoruz
                    _context.Products.Add(newProduct);
                    await _context.SaveChangesAsync();
                    return Ok(new { Message = "İlan başarıyla eklendi.",IlanID = newProduct.ProductID });
                }

            }
            catch (Exception ex)
            {
                // Hata ile ilgili daha fazla bilgi almak isterseniz ex.Message veya ex.StackTrace'i loglayabilirsiniz
                return Ok(new { Message = "İlan Teknik Bir Sorun Nedeniyle Eklenemedi." });
            }


        }

        [Route("ilanlarim/{ilanId}")]
        public async Task<IActionResult> ProductModify(int ilanId)
        {
            var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
            var productData = _context.Products.Where(x => x.OwnerUserID == loggedUser.Id && x.ProductID == ilanId && (x.Deleted == false || x.Deleted == null));
            var productsWithCategory = from p in productData
                                       where p.Deleted == false
                                       join c in _context.Categories on p.CategoryID equals c.CategoryID
                                       join city in _context.Citys on p.City equals city.Id
                                       join town in _context.Towns on p.District equals town.ID
                                       select new ProductMap
                                       {
                                           ProductID = p.ProductID,
                                           CategoryID = p.CategoryID,
                                           Name = p.Name,
                                           Capacity = p.Capacity,
                                           PricePerPerson = p.PricePerPerson,
                                           Text = p.Text,
                                           City = city.CityName,
                                           District = town.TownName,
                                           Images = p.Images,
                                           OwnerUserID = p.OwnerUserID,
                                           AproveByAdmin = p.AproveByAdmin,
                                           Sorting = p.Sorting,
                                           Deleted = p.Deleted ?? false,
                                           Visibility = p.Visibility ?? false,
                                           CategoryName = c.CategoryName // Assuming the category name is in the "Name" property of the Category table
                                       };
            var data = productsWithCategory.FirstOrDefault();
            return View(data);
        }
    }
}
