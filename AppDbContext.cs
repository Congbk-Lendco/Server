using Microsoft.EntityFrameworkCore;
using LendCoBEAPP.Models;
using LendCoBEAPP.Dtos;

namespace LendCoBEAPP
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Entity chính
        public DbSet<NguoiDung> NguoiDung { get; set; }

        // Dữ liệu từ stored procedure (không có khóa chính)
        public DbSet<VanBanDto> VanBanDtos { get; set; }
        public DbSet<VanBanDetail> VanBanDetail { get; set; }
        public DbSet<FileVanBanDto> FileVanBanDtos { get; set; }
        public DbSet<CommentDto> CommentDtos { get; set; }
        public DbSet<VanBanDetaiList> VanBanDetaiList { get; set; }
        public DbSet<LayThongTinNhanVien> ThongTinNhanVien { get; set; }
        public DbSet<Comment> Comments { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Cấu hình bảng cho entity Comment (quan trọng nếu bảng tên là 'Comment', không phải 'Comments')
    modelBuilder.Entity<Comment>().ToTable("Comment");

    // Các DTO dùng stored procedure hoặc view => không có khóa chính
    modelBuilder.Entity<VanBanDto>().HasNoKey();
    modelBuilder.Entity<FileVanBanDto>().HasNoKey();
    modelBuilder.Entity<CommentDto>().HasNoKey();
    modelBuilder.Entity<LayThongTinNhanVien>().HasNoKey();
    modelBuilder.Entity<VanBanDetail>().HasNoKey();
    modelBuilder.Entity<VanBanDetaiList>().HasNoKey();

    modelBuilder.Entity<NguoiDung>().HasKey(x => x.Id);
}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=localhost\\SQLEXPRESS;Database=LenCoDB;Trusted_Connection=True;TrustServerCertificate=True;"
                );
            }
        }
    }
}
