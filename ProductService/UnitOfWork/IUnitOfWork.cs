using ProductService.Repository;

namespace ProductService.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        Task CompleteAsync();
    }
}
