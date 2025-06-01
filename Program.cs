using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using LendCoBEAPP;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext với connection string đúng
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=LenCoDB;Trusted_Connection=True;TrustServerCertificate=True;"));

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
{
    policy.AllowAnyOrigin()   // cho phép tất cả origin (domain) gọi API
          .AllowAnyHeader()
          .AllowAnyMethod();
});

});

// Đăng ký controller
builder.Services.AddControllers();

// Cho phép sử dụng Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Sử dụng Swagger nếu môi trường là Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();      // CORS nên được đặt trước UseRouting
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
