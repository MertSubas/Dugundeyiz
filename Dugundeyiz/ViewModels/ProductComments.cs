using Dugundeyiz.Entity;

namespace Dugundeyiz.ViewModels
{
    public class CommentViewModel
    {
        public int CommentID { get; set; }
        public int SendToProductID { get; set; }
        public int? ReplyToCommentID { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public Boolean? Checking { get; set; }
    }
    public class ProductCommentPageViewModel
    {
        public List<CommentViewModel> Comments { get; set; }
        public int CurrentUserID { get; set; }
        public Product Ilan { get; set; }

    }


    public class FilterModel
    {
        public string IlanAdi { get; set; }
        public string Kapasite { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public string YildizSayisi { get; set; }
        public int categoryId { get; set; }


    }


}
