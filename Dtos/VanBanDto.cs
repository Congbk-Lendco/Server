namespace LendCoBEAPP.Dtos
{
   public class VanBanDto
{
    public string id_vanban { get; set; } = null!;
    public DateTime? ngayVB { get; set; }
    public string? soVB { get; set; }
    public string? noiphathanh { get; set; }
    public string? noidung { get; set; }
    public DateTime? ngaynhanVB { get; set; }
    public DateTime? ngaygiaoviec { get; set; }
    public DateTime? ngaydukienhoanthanh { get; set; }
    public string? ketqua { get; set; }
    public string? donvichutri { get; set; }
    public string? donviphoihop { get; set; }
    public string? nguoi_tao { get; set; } // phải đúng tên
    public string? ykienchidao { get; set; }
}

}
