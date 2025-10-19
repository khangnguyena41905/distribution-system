using AutoMapper;
using FluentValidation;
using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.AppUsers;

public record RegisterAppUserCommand(
    string UserName,
    string Password
) : ICommand<AppUser>;

internal class RegisterAppUserCommandValidator : AbstractValidator<RegisterAppUserCommand>
{
    public RegisterAppUserCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

internal class RegisterAppUserCommandHandler : ICommandHandler<RegisterAppUserCommand, AppUser>
{
    private readonly IAppUserRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    public RegisterAppUserCommandHandler(IUnitOfWork uow, IAppUserRepository repository,IMapper mapper)
    {
        _uow = uow; 
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<AppUser>> Handle(RegisterAppUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.FindSingleAsync(x => x.UserName == request.UserName);
        user.RegisterAccount(request.Password);
        await _repository.UpdateAsync(user);
        await _uow.CommitAsync();
        return Result<AppUser>.Success(user);
    }
}
