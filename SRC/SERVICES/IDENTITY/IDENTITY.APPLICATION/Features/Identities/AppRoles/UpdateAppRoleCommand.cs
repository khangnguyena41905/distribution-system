using FluentValidation;
using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;
using Microsoft.AspNetCore.Identity;

namespace IDENTITY.APPLICATION.Features.Identities.AppRoles;

internal class UpdateAppRoleCommandValidator : AbstractValidator<UpdateAppRoleCommand>
{
    public UpdateAppRoleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description không được để trống")
            .MaximumLength(256);

        RuleFor(x => x.RoleCode)
            .NotEmpty().WithMessage("RoleCode không được để trống")
            .MaximumLength(256);
    }
}
public record UpdateAppRoleCommand(
    Guid Id,
    string Description,
    string RoleCode,
    ICollection<IdentityUserRole<Guid>>? UserRoles,
    ICollection<IdentityRoleClaim<Guid>>? Claims,
    ICollection<Permission>? Permissions
) : ICommand<AppRole>;

internal class UpdateAppRoleCommandHandler : ICommandHandler<UpdateAppRoleCommand, AppRole>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppRoleRepository _appRoleRepository;
    private readonly IPermissionRepository _permissionRepository;
    
    public UpdateAppRoleCommandHandler(IUnitOfWork unitOfWork ,IAppRoleRepository appRoleRepository, IPermissionRepository permissionRepository)
    {
        _unitOfWork = unitOfWork;   
        _appRoleRepository = appRoleRepository;
        _permissionRepository = permissionRepository;
    }
    public async Task<Result<AppRole>> Handle(UpdateAppRoleCommand request, CancellationToken cancellationToken)
    {
        var appRole = await _appRoleRepository.FindByIdAsync(request.Id, x => x.Permissions);
        if (appRole is null)
            return Result.Failure<AppRole>(Error.NullValue);

        appRole.Description = request.Description;
        appRole.RoleCode = request.RoleCode;

        await _permissionRepository.RemoveByAppRoleIdAsync(request.Id);
        
        if (request.Permissions is not null)
            appRole.Permissions = request.Permissions;
        
        var result = await _appRoleRepository.UpdateAsync(appRole);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success(appRole);
    }
}