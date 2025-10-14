using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.AppRoles;

public record DeleteAppRoleCommand(Guid Id) : ICommand<bool>;

internal class DeleteAppRoleCommandHandler : ICommandHandler<DeleteAppRoleCommand, bool>
{
    private readonly IAppRoleRepository _appRoleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAppRoleCommandHandler(
        IAppRoleRepository appRoleRepository,
        IPermissionRepository permissionRepository,
        IUnitOfWork unitOfWork)
    {
        _appRoleRepository = appRoleRepository;
        _permissionRepository = permissionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteAppRoleCommand request, CancellationToken cancellationToken)
    {
        // 1. Lấy AppRole theo Id
        var appRole = await _appRoleRepository.FindByIdAsync(request.Id, x => x.UserRoles, x => x.Claims, x => x.Permissions);
        if (appRole is null)
            return Result.Failure<bool>(Error.NullValue);

        // 2. Xóa các Permission liên kết
        if (appRole.Permissions?.Any() == true)
        {
            await _permissionRepository.RemoveByAppRoleIdAsync(appRole.Id);
        }

        // 3. Xóa UserRoles (nếu không dùng cascade tự động)
        if (appRole.UserRoles?.Any() == true)
        {
            appRole.UserRoles.Clear(); // EF sẽ xóa các bản ghi liên kết
        }

        // 4. Xóa Claims (nếu không dùng cascade tự động)
        if (appRole.Claims?.Any() == true)
        {
            appRole.Claims.Clear();
        }

        // 5. Xóa chính AppRole
        await _appRoleRepository.RemoveAsync(appRole);

        // 6. Commit transaction
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success(true);
    }
}
