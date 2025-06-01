-- ================================
-- Script tạo cấu trúc và dữ liệu ví dụ hệ thống công ty phân quyền
-- Dùng cho SQL Server
-- Ngày tạo: 2025-05-29
-- ================================

-- XÓA BẢNG CŨ NẾU CÓ
DROP TABLE IF EXISTS LanhDaoChiNhanh;
DROP TABLE IF EXISTS NguoiDung;
DROP TABLE IF EXISTS PhongBan;
DROP TABLE IF EXISTS ChiNhanh;
DROP TABLE IF EXISTS VaiTro;

-- 1. VaiTro
CREATE TABLE VaiTro (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ten_vai_tro NVARCHAR(100) NOT NULL
);

-- 2. ChiNhanh
CREATE TABLE ChiNhanh (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ten_chi_nhanh NVARCHAR(100) NOT NULL
);

-- 3. NguoiDung
CREATE TABLE NguoiDung (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ho_ten NVARCHAR(100) NOT NULL,
    email NVARCHAR(100) NOT NULL UNIQUE,
    mat_khau NVARCHAR(255) NOT NULL,
    vai_tro_id UNIQUEIDENTIFIER NOT NULL,
    chi_nhanh_id UNIQUEIDENTIFIER NOT NULL,
    phong_ban_id UNIQUEIDENTIFIER NULL,
    ngay_bat_dau DATETIME NOT NULL,
    ngay_ket_thuc DATETIME NULL,

    CONSTRAINT FK_NguoiDung_VaiTro FOREIGN KEY (vai_tro_id) REFERENCES VaiTro(id),
    CONSTRAINT FK_NguoiDung_ChiNhanh FOREIGN KEY (chi_nhanh_id) REFERENCES ChiNhanh(id)
);

-- 4. PhongBan
CREATE TABLE PhongBan (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ten_phong_ban NVARCHAR(100) NOT NULL,
    chi_nhanh_id UNIQUEIDENTIFIER NOT NULL,
    truong_phong_id UNIQUEIDENTIFIER NULL,

    CONSTRAINT FK_PhongBan_ChiNhanh FOREIGN KEY (chi_nhanh_id) REFERENCES ChiNhanh(id),
    CONSTRAINT FK_PhongBan_TruongPhong FOREIGN KEY (truong_phong_id) REFERENCES NguoiDung(id)
);

-- 5. LanhDaoChiNhanh
CREATE TABLE LanhDaoChiNhanh (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    chi_nhanh_id UNIQUEIDENTIFIER NOT NULL,
    nguoi_dung_id UNIQUEIDENTIFIER NOT NULL,

    CONSTRAINT FK_LanhDaoChiNhanh_ChiNhanh FOREIGN KEY (chi_nhanh_id) REFERENCES ChiNhanh(id),
    CONSTRAINT FK_LanhDaoChiNhanh_NguoiDung FOREIGN KEY (nguoi_dung_id) REFERENCES NguoiDung(id)
);

-- ======================================
-- Chèn dữ liệu mẫu có liên kết rõ ràng
-- ======================================

-- ===== Khai báo biến GUID cho liên kết =====
DECLARE @vt_lanhdao UNIQUEIDENTIFIER = NEWID();
DECLARE @vt_truongphong UNIQUEIDENTIFIER = NEWID();
DECLARE @vt_nhanvien UNIQUEIDENTIFIER = NEWID();

DECLARE @cn_hn UNIQUEIDENTIFIER = NEWID();
DECLARE @cn_hcm UNIQUEIDENTIFIER = NEWID();
DECLARE @cn_bn UNIQUEIDENTIFIER = NEWID();

DECLARE @pb_hn UNIQUEIDENTIFIER = NEWID();
DECLARE @pb_hcm UNIQUEIDENTIFIER = NEWID();
DECLARE @pb_bn UNIQUEIDENTIFIER = NEWID();

-- 1. Vai trò
INSERT INTO VaiTro(id, ten_vai_tro) VALUES
(@vt_lanhdao, N'Lãnh đạo'),
(@vt_truongphong, N'Trưởng phòng'),
(@vt_nhanvien, N'Nhân viên');

-- 2. Chi nhánh
INSERT INTO ChiNhanh(id, ten_chi_nhanh) VALUES
(@cn_hn, N'Hà Nội'),
(@cn_hcm, N'Hồ Chí Minh'),
(@cn_bn, N'Bắc Ninh');

-- 3. Phòng ban (chưa có trưởng phòng)
INSERT INTO PhongBan(id, ten_phong_ban, chi_nhanh_id, truong_phong_id) VALUES
(@pb_hn, N'Phòng Kỹ Thuật HN', @cn_hn, NULL),
(@pb_hcm, N'Phòng Nhân Sự HCM', @cn_hcm, NULL),
(@pb_bn, N'Phòng Kinh Doanh BN', @cn_bn, NULL);

-- 4. Người dùng: Lãnh đạo
DECLARE @user_ld_hn UNIQUEIDENTIFIER = NEWID();
DECLARE @user_ld_hcm UNIQUEIDENTIFIER = NEWID();
DECLARE @user_ld_bn UNIQUEIDENTIFIER = NEWID();

INSERT INTO NguoiDung(id, ho_ten, email, mat_khau, vai_tro_id, chi_nhanh_id, phong_ban_id, ngay_bat_dau)
VALUES
(@user_ld_hn, N'Nguyễn Văn A', 'a@company.com', '123456', @vt_lanhdao, @cn_hn, NULL, GETDATE()),
(@user_ld_hcm, N'Trần Văn B', 'b@company.com', '123456', @vt_lanhdao, @cn_hcm, NULL, GETDATE()),
(@user_ld_bn, N'Lê Thị C', 'c@company.com', '123456', @vt_lanhdao, @cn_bn, NULL, GETDATE());

-- 5. Người dùng: Trưởng phòng
DECLARE @user_tp_hn UNIQUEIDENTIFIER = NEWID();
DECLARE @user_tp_hcm UNIQUEIDENTIFIER = NEWID();
DECLARE @user_tp_bn UNIQUEIDENTIFIER = NEWID();

INSERT INTO NguoiDung(id, ho_ten, email, mat_khau, vai_tro_id, chi_nhanh_id, phong_ban_id, ngay_bat_dau)
VALUES
(@user_tp_hn, N'Đặng Trưởng HN', 'tp_hn@company.com', '123456', @vt_truongphong, @cn_hn, @pb_hn, GETDATE()),
(@user_tp_hcm, N'Phạm Trưởng HCM', 'tp_hcm@company.com', '123456', @vt_truongphong, @cn_hcm, @pb_hcm, GETDATE()),
(@user_tp_bn, N'Bùi Trưởng BN', 'tp_bn@company.com', '123456', @vt_truongphong, @cn_bn, @pb_bn, GETDATE());

-- 6. Cập nhật lại trưởng phòng
UPDATE PhongBan SET truong_phong_id = @user_tp_hn WHERE id = @pb_hn;
UPDATE PhongBan SET truong_phong_id = @user_tp_hcm WHERE id = @pb_hcm;
UPDATE PhongBan SET truong_phong_id = @user_tp_bn WHERE id = @pb_bn;

-- 7. Nhân viên
INSERT INTO NguoiDung(id, ho_ten, email, mat_khau, vai_tro_id, chi_nhanh_id, phong_ban_id, ngay_bat_dau)
VALUES
(NEWID(), N'Nguyễn Nhân HN', 'nv_hn@company.com', '123456', @vt_nhanvien, @cn_hn, @pb_hn, GETDATE()),
(NEWID(), N'Lê Nhân HCM', 'nv_hcm@company.com', '123456', @vt_nhanvien, @cn_hcm, @pb_hcm, GETDATE()),
(NEWID(), N'Trần Nhân BN', 'nv_bn@company.com', '123456', @vt_nhanvien, @cn_bn, @pb_bn, GETDATE());

-- 8. Gán lãnh đạo cho chi nhánh
INSERT INTO LanhDaoChiNhanh(id, chi_nhanh_id, nguoi_dung_id) VALUES
(NEWID(), @cn_hn, @user_ld_hn),
(NEWID(), @cn_hcm, @user_ld_hcm),
(NEWID(), @cn_bn, @user_ld_bn);
