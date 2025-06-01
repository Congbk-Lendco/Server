using System;

namespace LendCoBEAPP.Dtos
{
    public class FileVanBanDto
    {
        public Guid FileId { get; set; }
        public Guid VanBanId { get; set; }  // Đổi từ string sang Guid nếu DB cũng là uniqueidentifier
        public string TenFile { get; set; }
        public string DuongDan { get; set; }
        public string LoaiFile { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
