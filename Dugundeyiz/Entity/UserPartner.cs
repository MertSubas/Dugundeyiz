using System.ComponentModel.DataAnnotations;

namespace Dugundeyiz.Entity
{
    public class UserPartner
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Adress { get; set; }
        public string CompanyName { get; set; }
        public int City { get; set; }
        public int District { get; set; }

    }

}
