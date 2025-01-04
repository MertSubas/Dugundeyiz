using Dugundeyiz.Entity;
using System.ComponentModel.DataAnnotations;

namespace Dugundeyiz.ViewModels
{
    public class CategoryListPageViewModel
    {
        public List<Category> Categories { get; set; }
    }
    public class CategoryViewModels
    {
        public string CategoryName { get; set; }
        public string? Image { get; set; }
    }
    public class CategoryFormUpdate
    {
        public int CategoryID { get; set; }
        public int? MainCategoryID { get; set; }
        public string? CategoryName { get; set; }
        public string? Image { get; set; }
        public Boolean? Deleted { get; set; }
        public int? Sorting { get; set; }
        public IFormFile uploadedImage { get; set; }

    }
    //public class CategoryManamentViewModel
    //{

    //    public int CategoryID { get; set; }
    //    public int? MainCategoryID { get; set; }
    //    public string? CategoryName { get; set; }
    //    public string? Image { get; set; }
    //    public Boolean? Deleted { get; set; }
    //    public int? Sorting { get; set; }
    //    public List<Category> Subcategory { get; set; }
    //}
    public class Newcategory
    {
        public string categoryName { get; set; }
        public IFormFile image { get; set; }
    }


    public class SortingUpdateModel
    {
        public int DraggedCategoryId { get; set; }
        public int NewSorting { get; set; }
    }
}
