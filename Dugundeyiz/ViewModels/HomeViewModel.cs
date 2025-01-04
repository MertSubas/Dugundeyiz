using Dugundeyiz.Entity;

namespace Dugundeyiz.ViewModels
{
    public class HomeViewModel
    {
        public List<Category> Kategoriler { get; set; }
        public List<Category> AltKategoriler { get; set; }
        public string Role { get; set; }
        //public List<SliderViewModel> Slider { get; set; }
        //public List<BlogPostViewModel> Posts { get; set; }
        //public BlogPostViewModel RandomPost { get; set; }
        //public List<SuggestionViewModel> Suggestions { get; set; }
    }
    public class HeaderUserViewModel
    {
        public string username { get; set; }
        public string name { get; set; }
        public int userID { get; set; }
        //public User user { get; set; }

    }
}
