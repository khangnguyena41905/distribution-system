using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using FluentValidation;
using INVENTORY.DOMAIN.Abstractions;
using INVENTORY.DOMAIN.Entities;
using INVENTORY.DOMAIN.Repositories;

namespace INVENTORY.APPLICATION.Features.Products;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price
) : ICommand<Product>;

internal class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên sản phẩm không được để trống")
            .MaximumLength(256);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Giá sản phẩm phải lớn hơn 0");
    }
}

internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Product>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Kiểm tra trùng tên sản phẩm (tuỳ theo rule business)
        var existing = await _productRepository.FindSingleAsync(x => x.Name == request.Name);
        if (existing is not null)
        {
            return Result.Failure<Product>(new Error("400", "Sản phẩm đã tồn tại"));
        }

        // Tạo product qua Aggregate method (có raise domain event)
        var product = Product.Create(request.Name, request.Description, request.Price);

        var result = await _productRepository.AddAsync(product);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success(result);
    }
}