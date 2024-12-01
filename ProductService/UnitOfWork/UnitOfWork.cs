using ProductService.DBContext;
using ProductService.Repository;

namespace ProductService.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductContext _context;
        public IProductRepository ProductRepository { get; }

        public UnitOfWork(ProductContext context)
        {
            _context = context;
            ProductRepository = new ProductRepository(_context);
        }

        public async Task CompleteAsync() => await _context.SaveChangesAsync();
    }
}
