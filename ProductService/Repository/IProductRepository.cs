using ProductService.Model;

namespace ProductService.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<SanPham>> GetAllProductsAsync();
        Task<SanPham> GetProductByIdAsync(int id);
        Task<IEnumerable<LoaiHang>> GetAllLoaiHangAsync();
        Task AddProductAsync(SanPham product);
        Task UpdateProductAsync(SanPham product);
        Task DeleteProductAsync(int id);
    }
}
