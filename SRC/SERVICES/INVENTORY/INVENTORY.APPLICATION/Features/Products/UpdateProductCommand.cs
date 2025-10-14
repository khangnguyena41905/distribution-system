using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using FluentValidation;
using INVENTORY.DOMAIN.Abstractions;
using INVENTORY.DOMAIN.Entities;
using INVENTORY.DOMAIN.Repositories;

namespace INVENTORY.APPLICATION.Features.Products;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price
) : ICommand<Product>;

internal class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id sản phẩm không được để trống");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên sản phẩm không được để trống")
            .MaximumLength(256);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Giá sản phẩm phải lớn hơn 0");
    }
}

internal class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, Product>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task<Result<Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByIdAsync(request.Id);

        if (product is null)
        {
            return Result.Failure<Product>(new Error("404", "Sản phẩm không tồn tại"));
        }
        
        product.GetType().GetProperty("Name")?.SetValue(product, request.Name);
        product.GetType().GetProperty("Description")?.SetValue(product, request.Description);
        product.GetType().GetProperty("Price")?.SetValue(product, request.Price);

        await _productRepository.UpdateAsync(product);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success(product);
    }
}