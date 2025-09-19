using FluentValidation;
using IDENTITY.CONTRACT.Abstractions.Message;
using IDENTITY.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN.Abstractions.Repositories.Accounts;
using IDENTITY.DOMAIN.Entities.Accounts;

namespace IDENTITY.APPLICATION.Features.Accounts;

public record CreateAccountCommand(string username, string password) : ICommand<Account>;

internal class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.username)
            .NotEmpty().WithMessage("username không được để trống");

        RuleFor(x => x.password)
            .NotEmpty().WithMessage("password không được để trống")
            .MaximumLength(256);
    }
}

internal class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, Account>
{
    private readonly IAccountRepository _accountRepository;

    public CreateAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<Result<Account>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = Account.Create(request.username, request.password);
        await _accountRepository.AddAsync(account);
        
        return Result<Account>.Success(account);    
    }
}