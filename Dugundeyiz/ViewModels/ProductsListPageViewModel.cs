using Dugundeyiz.Entity;
using System.ComponentModel.DataAnnotations;

namespace Dugundeyiz.ViewModels
{
    public class ProductListPageViewModel
    {
        public List<ProductMap> Products { get; set; }
        public Category Category { get; set; }
        public string ManName { get; set; }
        public int minPrice { get; set; }
        public int maxPrice { get; set; }
        //public List<SliderViewModel> Slider { get; set; }
        //public List<BlogPostViewModel> Posts { get; set; }
        //public BlogPostViewModel RandomPost { get; set; }
        //public List<SuggestionViewModel> Suggestions { get; set; }
    }

    public class ProductListByUser
    {
        public List<ProductMap> Products { get; set; }
        
        public int userID { get; set; }
        //public List<SliderViewModel> Slider { get; set; }
        //public List<BlogPostViewModel> Posts { get; set; }
        //public BlogPostViewModel RandomPost { get; set; }
        //public List<SuggestionViewModel> Suggestions { get; set; }
    }
    public class ProductMap

    {
        public int ProductID { get; set; }  
        public int CategoryID { get; set; }
        public int AproveByAdmin { get; set; }
        public string? Name { get; set; }
        public string? Capacity { get; set; }
        public int? PricePerPerson { get; set; }
        public int? CampaignPricePerPerson { get; set; }
        public string? Text { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Images { get; set; }
        public int OwnerUserID { get; set; }
        public int Sorting { get; set; }
        public string Video { get; set; }
        public Boolean Deleted { get; set; }
        public Boolean Visibility { get; set; }
        public string CategoryName { get; set; }
    }
}
