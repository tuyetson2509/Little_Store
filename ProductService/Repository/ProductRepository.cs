using Microsoft.EntityFrameworkCore;
using ProductService.DBContext;
using ProductService.Model;

namespace ProductService.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        public ProductRepository(ProductContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<LoaiHang>> GetAllLoaiHangAsync()
        {
            return await _context.LoaiHangs.ToListAsync();
        }
        public async Task<IEnumerable<SanPham>> GetAllProductsAsync() => await _context.SanPhams.ToListAsync();

        public async Task<SanPham> GetProductByIdAsync(int id) => await _context.SanPhams.FindAsync(id);

        public async Task AddProductAsync(SanPham product) => await _context.SanPhams.AddAsync(product);

        public async Task UpdateProductAsync(SanPham product) => _context.SanPhams.Update(product);

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.SanPhams.FindAsync(id);
            if (product != null) _context.SanPhams.Remove(product);
        }
    }
}
