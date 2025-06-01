namespace LendCoBEAPP.Dtos
{
    public class LayThongTinNhanVien
    {
        public Guid Id { get; set; } // 👈 Thêm dòng này
        public string? TenNhanVien { get; set; }
        public string? TenPhongBan { get; set; }
        public string? TenVaiTro { get; set; }
        public string? TenChiNhanh { get; set; }
        public string? Avatar { get; set; }
    }
}
