using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.AppRoles;

public record GetAllAppRoleQuery(int PageIndex = 1, int PageSize = 10) 
    : IQuery<PagedResult<AppRole>>;

internal class GetAllAppRoleQueryHandler 
    : IQueryHandler<GetAllAppRoleQuery, PagedResult<AppRole>>
{
    private readonly IAppRoleRepository _appRoleRepository;

    public GetAllAppRoleQueryHandler(IAppRoleRepository appRoleRepository)
    {
        _appRoleRepository = appRoleRepository;
    }

    public async Task<Result<PagedResult<AppRole>>> Handle(
        GetAllAppRoleQuery request, 
        CancellationToken cancellationToken)
    {

        var result = await _appRoleRepository.FindAllPagedAsync(
            pageIndex: request.PageIndex,
            pageSize: request.PageSize);

        return Result.Success(result);
    }
}