using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using INVENTORY.DOMAIN.Entities;
using INVENTORY.DOMAIN.Repositories;

namespace INVENTORY.APPLICATION.Features.Products;

public record GetProductByIdQuery(Guid Id) : IQuery<Product>;

internal class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Product>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<Product>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByIdAsync(request.Id);

        if (product is null)
            return Result.Failure<Product>(new Error("404", "Sản phẩm không tồn tại"));

        return Result.Success(product);
    }
}