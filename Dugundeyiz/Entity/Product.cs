using System.ComponentModel.DataAnnotations;

namespace Dugundeyiz.Entity
{
    public class Product

    {
        [Key]
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public int AproveByAdmin { get; set; }
        public string? Name { get; set; }
        public string? Capacity { get; set; }
        public int? PricePerPerson { get; set; }
        public int? CampaignPricePerPerson { get; set; }
        public string? Text { get; set; }
        public int? City { get; set; }
        public int? District { get; set; }
        public string? Images { get; set; }
        public string? DeliveryTime { get; set; }
        public string? Features { get; set; }
        public string? Views { get; set; }
        public string? Video { get; set; }
        public int OwnerUserID { get; set; }
        public int Sorting { get; set; }
        public Boolean? Deleted { get; set; }
        public Boolean? Visibility { get; set; }
    }
}
