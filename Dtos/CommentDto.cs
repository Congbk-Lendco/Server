namespace LendCoBEAPP.Dtos
{
    public class CommentDto
    {
        public Guid IdComment { get; set; }         // CommentId trong DB
        public string NoiDung { get; set; }         // Nội dung bình luận
        public DateTime NgayTao { get; set; }       // Thời gian bình luận (ThoiGian)
        public Guid? ReplyTo { get; set; }          // ParentCommentId (có thể null nếu không trả lời bình luận khác)
        public string TenNguoiDung { get; set; }   // Tên người dùng (join từ bảng NguoiDung)
        public string? AvatarNguoiDung { get; set; } // Link avatar, có thể null
    }
}
