using UserService.Repository;
using static UserService.Model.UserServiceModel;

namespace UserService.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        Task CompleteAsync();
    }
}
