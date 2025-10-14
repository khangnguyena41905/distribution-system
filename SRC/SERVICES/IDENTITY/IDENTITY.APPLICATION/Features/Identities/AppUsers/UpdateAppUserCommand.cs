using FluentValidation;
using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.AppUsers;

public record UpdateAppUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    DateTime? DayOfBirth,
    bool? IsDirector,
    bool? IsHeadOfDepartment,
    Guid? ManagerId,
    Guid PositionId,
    int IsReceipient,
    string UserName,
    string Email
) : ICommand<AppUser>;

internal class UpdateAppUserCommandValidator : AbstractValidator<UpdateAppUserCommand>
{
    public UpdateAppUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
    }
}

internal class UpdateAppUserCommandHandler : ICommandHandler<UpdateAppUserCommand, AppUser>
{
    private readonly IAppUserRepository _repository;
    private readonly IUnitOfWork _uow;
    public UpdateAppUserCommandHandler(IUnitOfWork uow, IAppUserRepository repository)
    {
        _uow = uow; 
        _repository = repository;
    }

    public async Task<Result<AppUser>> Handle(UpdateAppUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.FindByIdAsync(request.Id);

        if (user is null)
            return Result.Failure<AppUser>(new Error("400", "Người dùng không tồn tại."));

        user.Update(
            request.FirstName,
            request.LastName,
            request.DayOfBirth,
            request.IsDirector,
            request.IsHeadOfDepartment,
            request.ManagerId,
            request.PositionId,
            request.IsReceipient,
            request.UserName,
            request.Email
        );

        await _repository.UpdateAsync(user);
        await _uow.CommitAsync();
        return Result<AppUser>.Success(user);
    }
}
