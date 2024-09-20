using System.ComponentModel.DataAnnotations;

namespace Dugundeyiz.Entity
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public int Admin { get; set; }
        public int Role { get; set; }
        public Boolean Deleted { get; set; }
    }
}
