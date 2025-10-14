using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.AppUsers;

public record GetAppUserByIdQuery(Guid Id) : IQuery<AppUser>;

internal class GetAppUserByIdQueryHandler : IQueryHandler<GetAppUserByIdQuery, AppUser>
{
    private readonly IAppUserRepository _repository;

    public GetAppUserByIdQueryHandler(IAppUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<AppUser>> Handle(GetAppUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.FindByIdAsync(request.Id);

        if (user is null)
            return Result.Failure<AppUser>(Error.None);

        return Result<AppUser>.Success(user);
    }
}
