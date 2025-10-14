using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using Action = IDENTITY.DOMAIN.Entities.Identities.Action;

namespace IDENTITY.APPLICATION.Features.Identities.Actions;
public record GetActionByIdQuery(string Id) : IQuery<Action>;

internal class GetActionByIdQueryHandler : IQueryHandler<GetActionByIdQuery, Action>
{
    private readonly IActionRepository _actionRepository;

    public GetActionByIdQueryHandler(IActionRepository actionRepository)
    {
        _actionRepository = actionRepository;
    }

    public async Task<Result<Action>> Handle(GetActionByIdQuery request, CancellationToken cancellationToken)
    {
        var action = await _actionRepository.FindByIdAsync(request.Id);

        if (action is null)
            return Result.Failure<Action>(new Error("404", "Action not found"));

        return Result.Success(action);
    }
}