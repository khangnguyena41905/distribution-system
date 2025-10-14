using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.Actions;


public record DeleteActionCommand(string Id) : ICommand<bool>;

internal class DeleteActionCommandHandler : ICommandHandler<DeleteActionCommand, bool>
{
    private readonly IActionRepository _actionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteActionCommandHandler(IActionRepository actionRepository, IUnitOfWork unitOfWork)
    {
        _actionRepository = actionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteActionCommand request, CancellationToken cancellationToken)
    {
        var action = await _actionRepository.FindByIdAsync(request.Id);
        if (action is null)
            return Result.Failure<bool>(new Error("404", "Action not found"));

        await _actionRepository.RemoveAsync(action);
        await _unitOfWork.CommitAsync();

        return Result.Success(true);
    }
}