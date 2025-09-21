using IDENTITY.CONTRACT.Abstractions.Message;
using IDENTITY.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.AppUsers;

public record DeleteAppUserCommand(Guid Id) : ICommand<bool>;

internal class DeleteAppUserCommandHandler : ICommandHandler<DeleteAppUserCommand, bool>
{
    private readonly IAppUserRepository _repository;
    private readonly IUnitOfWork _uow;
    public DeleteAppUserCommandHandler(IUnitOfWork uow, IAppUserRepository repository)
    {
        _uow = uow; 
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(DeleteAppUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.FindByIdAsync(request.Id);

        if (user is null)
            return Result.Failure<bool>(Error.None);

        await _repository.RemoveAsync(user);
        await _uow.CommitAsync();
        return Result.Success(true);
    }
}