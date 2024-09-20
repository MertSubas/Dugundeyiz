using System.ComponentModel.DataAnnotations;

namespace Dugundeyiz.Entity
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public string? Image { get; set; }
        public Boolean? Deleted { get; set; }
    }
}
