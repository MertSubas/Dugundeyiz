using Dugundeyiz.Entity;

namespace Dugundeyiz.ViewModels
{
    public class AddProductPageViewModel
    {
        public Product Product { get; set; }
        public int UserID { get; set; }
        public List<IFormFile> Images { get; set; }
        public string ProductType { get; set; }

    }  
    public class ProductPageViewModel
    {
        public ProductMap Product { get; set; }
        public List<ProductPhoto> PhotoList { get; set; }

        public string Company { get; set; }
        //public int UserID { get; set; }
        //public List<IFormFile> Images { get; set; }
        //public string ProductType { get; set; }

    }


    public class AddProductViewModel
    {
        // Temel Bilgiler
        public string Title { get; set; } // İlan Başlığı
        public int Capacity { get; set; } // Kapasite
        public int Price { get; set; } // Kişi Başı Fiyat
        public string Description { get; set; } // İlan Açıklaması

        // Seçimler
        public int Category { get; set; } // Kategori ID
        public string City { get; set; } // Şehir ID
        public string District { get; set; } // İlçe ID
        public string DavetAlani { get; set; } // İlçe ID
        public string DeliveryTime { get; set; } // İlçe ID
        public List<string>Features { get; set; } // İlçe ID
        public List<string>FeaturesView { get; set; } // İlçe ID

        // İlan Tarihi
        public DateTime AdDate { get; set; } // İlan Tarihi

        // Medya
        public IFormFile? Video { get; set; } // Video Dosyası
        public IFormFile? CoverPhoto { get; set; } // Kapak Fotoğrafı
        public List<IFormFile>? Photos { get; set; } // Diğer Fotoğraflar
    }

}
