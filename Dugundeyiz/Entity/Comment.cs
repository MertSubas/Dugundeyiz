namespace Dugundeyiz.Entity
{
    public class Comment
    {
        public int ID { get; set; }
        public int SendToUserID { get; set; }
        public int SendToProductID { get; set; }
        public int IsSeen { get; set; }
        public string Message { get; set; } = string.Empty;
        public int UserID { get; set; }
        public DateTime CreateDate { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Rate { get; set; } = string.Empty;
        public bool Deleted { get; set; }
        public bool? Checking { get; set; }
        public int? ReplyToCommentID { get; set; } // Nullable as indicated by Allow Nulls
    }

}
