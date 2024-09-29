using System.ComponentModel.DataAnnotations;

namespace Dugundeyiz.Entity
{
    public class Product

    {
        [Key]
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public string? Name { get; set; }
        public string? Capacity { get; set; }
        public string? pricePerPerson { get; set; }
        public string? Text { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Images { get; set; }
        public int OwnerUserID { get; set; }
        public int Sorting { get; set; }
        public Boolean Deleted { get; set; }
    }
}
