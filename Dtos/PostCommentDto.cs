namespace LendCoBEAPP.Dtos
{
    public class PostCommentDto
    {
        public Guid VanBanId { get; set; }
        public Guid NguoiDungId { get; set; }
        public string NoiDung { get; set; } = string.Empty;
        public Guid? ReplyTo { get; set; }
    }
}
