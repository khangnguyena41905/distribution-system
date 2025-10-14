using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using Action = IDENTITY.DOMAIN.Entities.Identities.Action;

namespace IDENTITY.APPLICATION.Features.Identities.Actions;

public record GetActionsQuery() : IQuery<IEnumerable<Action>>;

internal class GetActionsQueryHandler : IQueryHandler<GetActionsQuery, IEnumerable<Action>>
{
    private readonly IActionRepository _actionRepository;

    public GetActionsQueryHandler(IActionRepository actionRepository)
    {
        _actionRepository = actionRepository;
    }

    public async Task<Result<IEnumerable<Action>>> Handle(GetActionsQuery request, CancellationToken cancellationToken)
    {
        var actions = await _actionRepository.FindAllAsync();
        return Result.Success(actions);
    }
}