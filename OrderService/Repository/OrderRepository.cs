using Microsoft.EntityFrameworkCore;
using OrderService.DBContext;
using static OrderService.Model.OrderModel;

namespace OrderService.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;

        public OrderRepository(OrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DonHang>> GetAllOrdersAsync() => await _context.DonHangs.ToListAsync();

        public async Task<DonHang> GetOrderByIdAsync(int id) => await _context.DonHangs.FindAsync(id);

        public async Task AddOrderAsync(DonHang order) => await _context.DonHangs.AddAsync(order);

        public async Task UpdateOrderAsync(DonHang order) => _context.DonHangs.Update(order);

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.DonHangs.FindAsync(id);
            if (order != null) _context.DonHangs.Remove(order);
        }
        public async Task<DonHang> GetByIdAsync(int id)
        {
            return await _context.DonHangs.FindAsync(id);
        }
        public async Task AddAsync(ChiTietDonHang entity)
        {
            await _context.ChiTietDonHangs.AddAsync(entity);
        }
        public DbContext GetDbContext()
        {
            return _context;
        }
    }
}
