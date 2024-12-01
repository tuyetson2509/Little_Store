using Microsoft.EntityFrameworkCore;
using static OrderService.Model.OrderModel;

namespace OrderService.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<DonHang>> GetAllOrdersAsync();
        Task<DonHang> GetOrderByIdAsync(int id);
        Task AddOrderAsync(DonHang order);
        Task UpdateOrderAsync(DonHang order);
        Task DeleteOrderAsync(int id);
        DbContext GetDbContext();
        Task<DonHang> GetByIdAsync(int id);
        Task AddAsync(ChiTietDonHang entity);
    }
}
