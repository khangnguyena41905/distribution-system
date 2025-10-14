using INVENTORY.DOMAIN.Entities;
using INVENTORY.DOMAIN.Repositories;
using INVENTORY.PERSISTENCE.Abstractions.DbContexts;

namespace INVENTORY.PERSISTENCE.Repositories;

internal class ProductRepository : RepositoryBase<Product, Guid>,IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }
}