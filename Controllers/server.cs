using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using LendCoBEAPP.Models;
using Microsoft.EntityFrameworkCore;
using LendCoBEAPP.Dtos;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using System.Linq;
using System;

namespace LendCoBEAPP.Controllers
{
    [ApiController]
    [Route("api/nv")]
    public class NhanVienController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public NhanVienController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("add-comment")]
public async Task<IActionResult> AddComment([FromBody] PostCommentDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.NoiDung))
        return BadRequest(new { message = "Nội dung không được để trống" });

    var comment = new Comment
    {
        CommentId = Guid.NewGuid(),
        VanBanId = dto.VanBanId,
        NguoiDungId = dto.NguoiDungId,
        NoiDung = dto.NoiDung.Trim(),
        ThoiGian = DateTime.Now,
        ParentCommentId = dto.ReplyTo
    };

    try
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "Lỗi lưu comment", detail = ex.Message });
    }

    var nguoiDung = await _context.ThongTinNhanVien
        .Where(u => u.Id == dto.NguoiDungId)
        .Select(u => new
        {
            TenNguoiDung = u.TenNhanVien,
            u.Avatar
        })
        .FirstOrDefaultAsync();

    return StatusCode(201, new
    {
        idComment = comment.CommentId,
        noiDung = comment.NoiDung,
        ngayTao = comment.ThoiGian,
        replyTo = comment.ParentCommentId,
        tenNguoiDung = nguoiDung?.TenNguoiDung ?? "Ẩn danh",
        avatarNguoiDung = nguoiDung?.Avatar
    });
}

        [HttpGet("nhanvien")]
        public async Task<IActionResult> GetThongTinNhanVien()
        {
            try
            {
                var result = await _context.ThongTinNhanVien
                    .FromSqlRaw("EXEC dbo.sp_LayThongTinNhanVien")
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi lấy thông tin nhân viên", detail = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> DangNhap([FromBody] LoginDto request)
        {
            var list = await _context.ThongTinNhanVien
                .FromSqlRaw("EXEC sp_KiemTraDangNhap @Email = {0}, @MatKhau = {1}", request.Email, request.MatKhau)
                .ToListAsync();

            var result = list.FirstOrDefault();

            if (result == null)
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng." });

            return Ok(result);
        }

        [HttpPost("vanban/details")]
        public async Task<IActionResult> GetVanBanDetails([FromBody] VanBanRequestDto request)
        {
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                await conn.OpenAsync();

                using var multi = await conn.QueryMultipleAsync(
                    "dbo.sp_GetVanBanDetails",
                    new { IdVanBan = request.IdVanBan },
                    commandType: CommandType.StoredProcedure);

                var vanBanDetail = await multi.ReadFirstOrDefaultAsync<VanBanDetaiList>();
                if (vanBanDetail == null)
                    return NotFound("Không tìm thấy văn bản");

                var fileVanBanList = (await multi.ReadAsync<FileVanBanDto>()).ToList();
                var commentList = (await multi.ReadAsync<CommentDto>()).ToList();

                vanBanDetail.FileVanBanList = fileVanBanList;
                vanBanDetail.CommentList = commentList;

                return Ok(vanBanDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi lấy chi tiết văn bản", detail = ex.Message });
            }
        }

        [HttpGet("vanban")]
        public async Task<IActionResult> GetVanBan()
        {
            try
            {
                var result = await _context.VanBanDtos
                    .FromSqlRaw("EXEC dbo.sp_LayVanBan_FullInfo")
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi lấy thông tin văn bản", detail = ex.Message });
            }
        }
    }
}
