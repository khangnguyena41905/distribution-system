using IDENTITY.CONTRACT.Abstractions.Message;
using IDENTITY.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.Functions;

public record GetFunctionByIdQuery(string Id) : IQuery<Function>;

internal class GetFunctionByIdQueryHandler : IQueryHandler<GetFunctionByIdQuery, Function>
{
    private readonly IFunctionRepository _functionRepository;

    public GetFunctionByIdQueryHandler(IFunctionRepository functionRepository)
    {
        _functionRepository = functionRepository;
    }

    public async Task<Result<Function>> Handle(GetFunctionByIdQuery request, CancellationToken cancellationToken)
    {
        var func = await _functionRepository.FindByIdAsync(request.Id);

        if (func is null)
            return Result.Failure<Function>(new Error("404", "Function not found"));

        return Result.Success(func);
    }
}
