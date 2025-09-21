using IDENTITY.CONTRACT.Abstractions.Message;
using IDENTITY.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.Permissions;

public record CheckUserPermissionQuery(Guid UserId, string FunctionId, string ActionId) : IQuery<bool>;

internal class CheckUserPermissionQueryHandler : IQueryHandler<CheckUserPermissionQuery, bool>
{
    private readonly IAppUserRepository _userRepository;

    public CheckUserPermissionQueryHandler(IAppUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<bool>> Handle(CheckUserPermissionQuery request, CancellationToken cancellationToken)
    {
        var (user, roles) = await _userRepository.GetUserWithRolesAsync(request.UserId);
        
        if (user == null) 
            return Result.Failure<bool>(Error.None);

        foreach (var role in roles)
        {
            var hasPermission = role.Permissions.Any(p =>
                p.FunctionId == request.FunctionId &&
                p.ActionId == request.ActionId
            );

            if (hasPermission) 
                return Result<bool>.Success(true);
        }

        return Result<bool>.Success(false);
    }

}
