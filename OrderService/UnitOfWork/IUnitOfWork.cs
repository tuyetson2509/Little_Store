using Microsoft.EntityFrameworkCore;
using OrderService.Repository;

namespace OrderService.UnitOfWork
{
    public interface IUnitOfWork
    {
        IOrderRepository OrderRepository { get; }
        DbContext GetDbContext();
        Task CompleteAsync();
    }
}
