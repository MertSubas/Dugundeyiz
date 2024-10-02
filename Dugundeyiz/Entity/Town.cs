using System.ComponentModel.DataAnnotations;

namespace Dugundeyiz.Entity
{
    public class Town
    {
        [Key]
        public int ID { get; set; }
        public string TownName { get; set; }
        public int CityID { get; set; }
    }
}
