using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Model
{
    public class UserServiceModel
    {
        public class TaiKhoan
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Đảm bảo MaNguoiDung tự động tăng
            public int MaNguoiDung { get; set; }

            public string HoTen { get; set; }
            public string Email { get; set; }
            public string Dienthoai { get; set; }
            public string Matkhau { get; set; }

            [DefaultValue(2)]  // Gán mặc định IDQuyen là 2 nếu không được chỉ định
            public int IDQuyen { get; set; }

            public string Diachi { get; set; }
        }

        public class PhanQuyen
        {
            public int Id { get; set; }
            public string RoleName { get; set; }
        }
        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        public class LoginResponse
        {
            public string Token { get; set; }
            public TaiKhoan User { get; set; }
        }
        public class TaiKhoanDangKy
        {
            public string HoTen { get; set; }
            public string Email { get; set; }
            public string Dienthoai { get; set; }
            public string Matkhau { get; set; }
            public string Diachi { get; set; }
        }
    }
}
