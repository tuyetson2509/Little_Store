using static UserService.Model.UserServiceModel;
using UserService.DBContext;
using Microsoft.EntityFrameworkCore;

namespace UserService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaiKhoan>> GetAllUsersAsync() => await _context.TaiKhoans.ToListAsync();

        public async Task<TaiKhoan> GetUserByIdAsync(int id) => await _context.TaiKhoans.FindAsync(id);

        public async Task AddUserAsync(TaiKhoan user) => await _context.TaiKhoans.AddAsync(user);

        public async Task UpdateUserAsync(TaiKhoan user) => _context.TaiKhoans.Update(user);

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.TaiKhoans.FindAsync(id);
            if (user != null) _context.TaiKhoans.Remove(user);
        }
        public async Task<TaiKhoan> GetUserByUsernameAsync(string username)
        {
            return await _context.Set<TaiKhoan>().FirstOrDefaultAsync(u => u.Email == username);
        }
    }
}
