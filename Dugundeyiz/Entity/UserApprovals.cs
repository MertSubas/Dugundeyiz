using System.ComponentModel.DataAnnotations;

namespace Dugundeyiz.Entity
{
    public partial class UserApproval
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ApprovalToken { get; set; }
        public DateTime ApprovalRequestedAt { get; set; }

        //public virtual User User { get; set; }
    }

}
