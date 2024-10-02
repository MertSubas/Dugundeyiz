using System.ComponentModel.DataAnnotations;

namespace Dugundeyiz.Entity
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string CityName { get; set; }
    }
}
