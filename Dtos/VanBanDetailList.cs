using System;
using System.Collections.Generic;

namespace LendCoBEAPP.Dtos
{
    public class VanBanDetaiList
    {
        public Guid IdVanBan { get; set; }
        public string NoiDung { get; set; }
        public string NoiPhatHanh { get; set; }
        public DateTime NgayVB { get; set; }

        // Thêm thông tin người tạo
        public Guid? NguoiTaoId { get; set; }
        public string TenNguoiTao { get; set; }
        public string AvatarNguoiTao { get; set; }

        public List<FileVanBanDto> FileVanBanList { get; set; }
        public List<CommentDto> CommentList { get; set; }
    }
}
