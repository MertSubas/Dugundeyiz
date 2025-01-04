using Dugundeyiz.Context;
using Dugundeyiz.ViewModels;

namespace Dugundeyiz.Services
{
    public class ProductService
    {
        private readonly DugundeyizContext _context;

        public ProductService(DugundeyizContext context)
        {
            _context = context;
        }
        public IQueryable<ProductMap> GetProductsWithCategory(string categoryName)
        {
            return from p in _context.Products
                   where p.Deleted == false && p.Visibility != false
                   join c in _context.Categories on p.CategoryID equals c.CategoryID
                   where c.CategoryName == categoryName && c.Deleted != true
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
                       CategoryName = c.CategoryName
                   };
        }

        public (int MaxPrice, int MinPrice) GetMinMaxPrices(IQueryable<ProductMap> products)
        {
            var maxPricePerPerson = products.Max(x => x.PricePerPerson ?? 0);
            var maxCampaignPricePerPerson = products.Max(x => x.CampaignPricePerPerson ?? 0);
            var maxPrice = Math.Max(maxPricePerPerson, maxCampaignPricePerPerson);

            //var minPricePerPerson = products
            //    .Where(x => x.PricePerPerson != null && x.PricePerPerson > 0)
            //    .Min(x => x.PricePerPerson.Value);

            //var minCampaignPricePerPerson = products
            //    .Where(x => x.CampaignPricePerPerson != null && x.CampaignPricePerPerson > 0)
            //    .Min(x => x.CampaignPricePerPerson.Value);
            var minPricePerPerson = products.Min(x => x.PricePerPerson ?? 0);
            var minCampaignPricePerPerson = products.Min(x => x.CampaignPricePerPerson ?? 0);
            if(minCampaignPricePerPerson==0)
            {
                minCampaignPricePerPerson = minPricePerPerson;
            }
            var minPrice = Math.Min(minPricePerPerson, minCampaignPricePerPerson);

            return (maxPrice, minPrice);
        }

        public List<ProductMap> FilterProducts(FilterModel filters)
        {
            // Kullanıcı kapasite girdisini işleme
            int.TryParse(filters.Kapasite, out int filterCapacity);

            // Ana query
            var query = _context.Products
                .Where(p => p.Deleted == false && p.Visibility != false &&
                            (filters.categoryId == 0 || p.CategoryID == filters.categoryId)) // Kategori kontrolü burada
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

            // İlan adı filtreleme
            if (!string.IsNullOrEmpty(filters.IlanAdi) && filters.IlanAdi != "")
            {
                query = query.Where(x => x.Product.Name.Contains(filters.IlanAdi));
            }

            // Şehir ve ilçe filtreleme
            if (!string.IsNullOrEmpty(filters.City) && filters.City != "Şehir")
            {
                query = query.Where(x => x.City.CityName == filters.City);
            }

            if (!string.IsNullOrEmpty(filters.District) && filters.District != "İlçe")
            {
                query = query.Where(x => x.Town.TownName == filters.District);
            }

            // Fiyat filtreleme
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

            // Sonuçları dönüştürme
            var data= query.Select(x => new ProductMap
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


            // Kapasiteyi bellekte filtrele
            if (!string.IsNullOrEmpty(filters.Kapasite) && filters.Kapasite != "")
            {
                data = data.Where(p =>
                    string.IsNullOrEmpty(p.Capacity) || // Kapasite null olanları dahil et
                    p.Capacity == filters.Kapasite ||  // Aynı kapasiteye sahip ürünleri dahil et
                    (p.Capacity.Contains("-") &&       // Kapasite bir aralık ise
                     p.Capacity.Split('-').Length == 2 &&
                     int.TryParse(p.Capacity.Split('-')[0].Trim(), out int minCapacity) &&
                     int.TryParse(p.Capacity.Split('-')[1].Trim(), out int maxCapacity) &&
                     filterCapacity >= minCapacity && filterCapacity <= maxCapacity)).ToList(); // Aralığa uyuyor mu
            }

            return data;

        }

    }
}
