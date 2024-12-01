using static UserService.Model.UserServiceModel;

namespace UserService.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<TaiKhoan>> GetAllUsersAsync();
        Task<TaiKhoan> GetUserByIdAsync(int id);
        Task AddUserAsync(TaiKhoan user);
        Task UpdateUserAsync(TaiKhoan user);
        Task DeleteUserAsync(int id);
        Task<TaiKhoan> GetUserByUsernameAsync(string username);
    }
}
