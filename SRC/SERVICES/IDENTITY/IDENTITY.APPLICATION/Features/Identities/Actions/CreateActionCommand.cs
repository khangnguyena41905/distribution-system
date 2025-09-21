using FluentValidation;
using IDENTITY.CONTRACT.Abstractions.Message;
using IDENTITY.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using Action = IDENTITY.DOMAIN.Entities.Identities.Action;

namespace IDENTITY.APPLICATION.Features.Identities.Actions;

public record CreateActionCommand(
    string Id,
    string Name,
    int? SortOrder,
    bool? IsActive
) : ICommand<Action>;

internal class CreateActionCommandValidator : AbstractValidator<CreateActionCommand>
{
    public CreateActionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên action không được để trống")
            .MaximumLength(256);

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).When(x => x.SortOrder.HasValue);

    }
}

internal class CreateActionCommandHandler : ICommandHandler<CreateActionCommand, Action>
{
    private readonly IActionRepository _actionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateActionCommandHandler(IUnitOfWork unitOfWork, IActionRepository actionRepository)
    {
        _unitOfWork = unitOfWork;
        _actionRepository = actionRepository;
    }

    public async Task<Result<Action>> Handle(CreateActionCommand request, CancellationToken cancellationToken)
    {
        var existing = await _actionRepository
            .FindSingleAsync(x => x.Name == request.Name);

        if (existing is not null)
        {
            return Result.Failure<Action>(new Error("400", "Existed an existing action"));
        }

        var action = new Action
        {
            Id = request.Id,
            Name = request.Name,
            SortOrder = request.SortOrder,
            IsActive = request.IsActive ?? true
        };

        var result = await _actionRepository.AddAsync(action);
        await _unitOfWork.CommitAsync();
        return Result.Success(result);
    }
}
