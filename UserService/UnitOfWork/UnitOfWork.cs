using Microsoft.EntityFrameworkCore;
using UserService.DBContext;
using UserService.Repository;
using static UserService.Model.UserServiceModel;

namespace UserService.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserContext _context;
        public IUserRepository UserRepository { get; }

        public UnitOfWork(UserContext context)
        {
            _context = context;
            UserRepository = new UserRepository(_context);
        }
        public async Task CompleteAsync() => await _context.SaveChangesAsync();
    }
}
