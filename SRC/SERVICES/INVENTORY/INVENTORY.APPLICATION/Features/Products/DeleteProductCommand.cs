using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using INVENTORY.DOMAIN.Abstractions;
using INVENTORY.DOMAIN.Repositories;

namespace INVENTORY.APPLICATION.Features.Products;

public record DeleteProductCommand(Guid Id) : ICommand<bool>;

internal class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task<Result<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByIdAsync(request.Id);

        if (product is null)
        {
            return Result.Failure<bool>(new Error("404", "Sản phẩm không tồn tại"));
        }

        await _productRepository.VirtualRemoveAsync(product);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success(true);
    }
}