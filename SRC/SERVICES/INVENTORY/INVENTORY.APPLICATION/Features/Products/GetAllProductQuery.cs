using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using INVENTORY.DOMAIN.Entities;
using INVENTORY.DOMAIN.Repositories;

namespace INVENTORY.APPLICATION.Features.Products;

public record GetAllProductQuery(int PageIndex = 1, int PageSize = 10) 
    : IQuery<PagedResult<Product>>;

internal class GetAllProductQueryHandler 
    : IQueryHandler<GetAllProductQuery, PagedResult<Product>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<PagedResult<Product>>> Handle(
        GetAllProductQuery request, 
        CancellationToken cancellationToken)
    {
        var result = await _productRepository.FindAllPagedAsync(
            pageIndex: request.PageIndex,
            pageSize: request.PageSize);

        return Result.Success(result);
    }
}