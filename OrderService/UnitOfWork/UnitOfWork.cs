using Microsoft.EntityFrameworkCore;
using OrderService.DBContext;
using OrderService.Repository;

namespace OrderService.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderContext _context;
        public IOrderRepository OrderRepository { get; }
        public UnitOfWork(OrderContext context)
        {
            _context = context;
            OrderRepository = new OrderRepository(_context);
        }
        public DbContext GetDbContext()
        {
            return _context;
        }
        public async Task CompleteAsync() => await _context.SaveChangesAsync();
    }
}
