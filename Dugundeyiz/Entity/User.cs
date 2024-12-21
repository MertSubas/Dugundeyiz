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
        public string Email { get; set; }
        public bool Admin { get; set; }
        public Roles Role { get; set; }
        public bool Deleted { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? WeddingDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
   public  enum Roles
    {
        Admin,User,Partner

    }
}
