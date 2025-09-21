using IDENTITY.CONTRACT.Abstractions.Message;
using IDENTITY.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.Permissions;

public record AssignPermissionsToRoleCommand(Guid RoleId, List<(string FunctionId, string ActionId)> Permissions) : ICommand<bool>;

internal class AssignPermissionsToRoleCommandHandler : ICommandHandler<AssignPermissionsToRoleCommand, bool>
{
    private readonly IAppRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignPermissionsToRoleCommandHandler(IUnitOfWork unitOfWork, IAppRoleRepository roleRepository)
    {
        _unitOfWork = unitOfWork;   
        _roleRepository = roleRepository;
    }

    public async Task<Result<bool>> Handle(AssignPermissionsToRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.FindByIdAsync(request.RoleId);
        if (role == null) return Result.Failure<bool>(Error.None);

        role.Permissions = request.Permissions
            .Select(p => new Permission
            {
                RoleId = role.Id,
                FunctionId = p.FunctionId,
                ActionId = p.ActionId
            }).ToList();

        await _roleRepository.UpdateAsync(role);
        await _unitOfWork.CommitAsync();
        return Result<bool>.Success(true);
    }
}
