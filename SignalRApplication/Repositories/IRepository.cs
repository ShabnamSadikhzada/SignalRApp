using SignalRApplication.Models;

namespace SignalRApplication.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    IEnumerable<T> GetAll();
}


public interface IProductRepository : IRepository<Product> { }
public interface ICategoryRepository : IRepository<Category> { }