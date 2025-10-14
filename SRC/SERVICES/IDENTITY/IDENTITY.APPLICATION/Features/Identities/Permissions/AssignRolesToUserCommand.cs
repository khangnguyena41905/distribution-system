using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.Permissions;

public record AssignRolesToUserCommand(Guid UserId, List<Guid> RoleIds) : ICommand<bool>;

internal class AssignRolesToUserCommandHandler : ICommandHandler<AssignRolesToUserCommand, bool>
{
    private readonly IAppUserRepository _userRepository;
    private readonly IAppRoleRepository _roleRepository;

    public AssignRolesToUserCommandHandler(IAppUserRepository userRepository, IAppRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<Result<bool>> Handle(AssignRolesToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.UserId);
        if (user == null) return Result.Failure<bool>(Error.NullValue);

        foreach (var roleId in request.RoleIds)
        {
            var role = await _roleRepository.FindByIdAsync(roleId);
            if (role == null) continue;

            await _userRepository.AssignRoleAsync(user, role); // method to handle UserRoles
        }

        return Result<bool>.Success(true);
    }
}
