namespace LendCoBEAPP.Models
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public Guid VanBanId { get; set; }
        public Guid NguoiDungId { get; set; }
        public string NoiDung { get; set; }
        public DateTime ThoiGian { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
