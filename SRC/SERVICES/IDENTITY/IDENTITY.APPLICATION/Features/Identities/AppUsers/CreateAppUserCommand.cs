using FluentValidation;
using IDENTITY.CONTRACT.Abstractions.Message;
using IDENTITY.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.AppUsers;

public record CreateAppUserCommand(
    string FirstName,
    string LastName,
    DateTime? DayOfBirth,
    bool? IsDirector,
    bool? IsHeadOfDepartment,
    Guid? ManagerId,
    Guid PositionId,
    int IsReceipient,
    Guid AccountId,
    string UserName,
    string Email
) : ICommand<AppUser>;

internal class CreateAppUserCommandValidator : AbstractValidator<CreateAppUserCommand>
{
    public CreateAppUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.PositionId).NotEmpty();
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
    }
}

internal class CreateAppUserCommandHandler : ICommandHandler<CreateAppUserCommand, AppUser>
{
    private readonly IAppUserRepository _repository;

    public CreateAppUserCommandHandler(IAppUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<AppUser>> Handle(CreateAppUserCommand request, CancellationToken cancellationToken)
    {
        var user = AppUser.Create(
            request.FirstName,
            request.LastName,
            request.DayOfBirth,
            request.IsDirector,
            request.IsHeadOfDepartment,
            request.ManagerId,
            request.PositionId,
            request.IsReceipient,
            request.AccountId,
            request.UserName,
            request.Email
        );

        await _repository.AddAsync(user);

        return Result<AppUser>.Success(user);
    }
}
