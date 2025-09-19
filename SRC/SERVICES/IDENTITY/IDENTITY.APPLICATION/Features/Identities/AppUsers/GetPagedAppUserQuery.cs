using System.Linq.Expressions;
using IDENTITY.CONTRACT.Abstractions.Message;
using IDENTITY.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.AppUsers;

public record GetPagedAppUserQuery(
    int PageIndex,
    int PageSize,
    string? Search 
) : IQuery<PagedResult<AppUser>>;


internal class GetPagedAppUserQueryHandler : IQueryHandler<GetPagedAppUserQuery, PagedResult<AppUser>>
{
    private readonly IAppUserRepository _repository;

    public GetPagedAppUserQueryHandler(IAppUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResult<AppUser>>> Handle(GetPagedAppUserQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<AppUser, bool>>? predicate = null;

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();

            predicate = x =>
                x.FirstName.ToLower().Contains(search) ||
                x.LastName.ToLower().Contains(search) ||
                x.Email.ToLower().Contains(search) ||
                x.UserName.ToLower().Contains(search);
        }

        var result = await _repository.FindAllPagedAsync(
            request.PageIndex,
            request.PageSize,
            predicate,
            x => x.Account, // include Account if needed
            x => x.UserRoles // include roles if needed
        );

        return Result<PagedResult<AppUser>>.Success(result);
    }
}
