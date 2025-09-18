using IDENTITY.CONTRACT.Abstractions.Message;
using IDENTITY.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.Functions;

public record GetFunctionsQuery() : IQuery<IEnumerable<Function>>;

internal class GetFunctionsQueryHandler : IQueryHandler<GetFunctionsQuery, IEnumerable<Function>>
{
    private readonly IFunctionRepository _functionRepository;

    public GetFunctionsQueryHandler(IFunctionRepository functionRepository)
    {
        _functionRepository = functionRepository;
    }

    public async Task<Result<IEnumerable<Function>>> Handle(GetFunctionsQuery request, CancellationToken cancellationToken)
    {
        var functions = await _functionRepository.FindAllAsync();
        return Result.Success(functions);
    }
}
