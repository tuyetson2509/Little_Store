using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static UserService.Model.UserServiceModel;
using UserService.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }



        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] TaiKhoanDangKy user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra xem tài khoản đã tồn tại chưa
            var existingUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(user.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Username already exists." });
            }

            // Sử dụng PasswordHasher để mã hóa mật khẩu
            var passwordHasher = new PasswordHasher<TaiKhoanDangKy>();
            var hashedPassword = passwordHasher.HashPassword(user, user.Matkhau);

            var taiKhoan = new TaiKhoan
            {
                HoTen = user.HoTen,
                Email = user.Email,
                Dienthoai = user.Dienthoai,
                Matkhau = hashedPassword, // Lưu mật khẩu đã mã hóa
                Diachi = user.Diachi,
                IDQuyen = 2
            };

            // Tiến hành lưu người dùng vào cơ sở dữ liệu
            await _unitOfWork.UserRepository.AddUserAsync(taiKhoan);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Registration successful." });
        }

        // API đăng nhập
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // Kiểm tra thông tin đăng nhập
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(loginRequest.Username);
            if (user == null)
            {
                return Unauthorized("Invalid Username");
            }

            var storedHash = Convert.FromBase64String(user.Matkhau);
            // Sử dụng PasswordHasher để kiểm tra mật khẩu
            var passwordHasher = new PasswordHasher<TaiKhoan>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Matkhau, loginRequest.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid Password");
            }


            // Tạo token JWT
            var token = GenerateJwtToken(user);
            var userinfo = new TaiKhoan
            {
                MaNguoiDung = user.MaNguoiDung,
                HoTen = user.HoTen,
                Email = user.Email,
                Dienthoai = user.Dienthoai,
                IDQuyen = user.IDQuyen,
                Diachi = user.Diachi
            };
            var response = new LoginResponse
            {
                Token = token,
                User = userinfo
            };
            return Ok(response); // Chỉ trả về token
        }
        [HttpPut("Edit")]
        //[ValidateAntiForgeryToken]
        //[AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditUser([FromBody] TaiKhoan updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Kiểm tra xem người dùng có tồn tại không
            var existingUser = await _unitOfWork.UserRepository.GetUserByIdAsync(updatedUser.MaNguoiDung);
            if (existingUser == null)
            {
                return NotFound(new { message = "User not found." });
            }
            // Sử dụng PasswordHasher để mã hóa mật khẩu
            var passwordHasher = new PasswordHasher<TaiKhoan>();
            var hashedPassword = passwordHasher.HashPassword(existingUser, updatedUser.Matkhau);
            // Cập nhật thông tin người dùng
            existingUser.HoTen = updatedUser.HoTen;
            existingUser.Email = updatedUser.Email;
            existingUser.Dienthoai = updatedUser.Dienthoai;
            existingUser.Diachi = updatedUser.Diachi;
            existingUser.Matkhau = hashedPassword; // Nếu cho phép cập nhật mật khẩu

            // Lưu thay đổi vào cơ sở dữ liệu
            _unitOfWork.UserRepository.UpdateUserAsync(existingUser);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "User updated successfully." });
        }

        // Tạo JWT token
        private string GenerateJwtToken(TaiKhoan user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
