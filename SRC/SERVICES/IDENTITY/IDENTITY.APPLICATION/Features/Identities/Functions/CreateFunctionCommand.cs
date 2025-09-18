using IDENTITY.CONTRACT.Abstractions.Message;
using IDENTITY.DOMAIN.Entities.Identities;
using FluentValidation;
using IDENTITY.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.Functions;

public record CreateFunctionCommand(
    string Name,
    string Url,
    string? ParrentId,
    int? SortOrder,
    string? CssClass,
    bool? IsActive
) : ICommand<Function>;

internal class CreateFunctionCommandValidator : AbstractValidator<CreateFunctionCommand>
{
    public CreateFunctionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên function không được để trống")
            .MaximumLength(256);

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Url không được để trống")
            .MaximumLength(500);

        RuleFor(x => x.CssClass)
            .MaximumLength(100);
    }
}

internal class CreateFunctionCommandHandler : ICommandHandler<CreateFunctionCommand, Function>
{
    private readonly IFunctionRepository _functionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFunctionCommandHandler(IFunctionRepository functionRepository, IUnitOfWork unitOfWork)
    {
        _functionRepository = functionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Function>> Handle(CreateFunctionCommand request, CancellationToken cancellationToken)
    {
        // Kiểm tra trùng Name
        var existing = await _functionRepository.FindSingleAsync(x => x.Name == request.Name);
        if (existing is not null)
            return Result.Failure<Function>(new Error("400", "Function name already exists"));

        var func = new Function
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Url = request.Url,
            ParrentId = request.ParrentId,
            SortOrder = request.SortOrder,
            CssClass = request.CssClass,
            IsActive = request.IsActive ?? true
        };

        var result = await _functionRepository.AddAsync(func);
        await _unitOfWork.CommitAsync();
        return Result.Success(result);
    }
}
