using INVENTORY.DOMAIN.Abstractions.Repositories;
using INVENTORY.DOMAIN.Entities;

namespace INVENTORY.DOMAIN.Repositories;

public interface IProductRepository: IRepositoryBase<Product, Guid>
{
    
}