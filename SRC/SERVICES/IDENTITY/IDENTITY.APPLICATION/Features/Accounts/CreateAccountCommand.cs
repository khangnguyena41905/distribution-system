using FluentValidation;
using IDENTITY.CONTRACT.Abstractions.Message;
using IDENTITY.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN;
using IDENTITY.DOMAIN.Abstractions.Repositories.Accounts;
using IDENTITY.DOMAIN.Entities.Accounts;

namespace IDENTITY.APPLICATION.Features.Accounts;

public record CreateAccountCommand(string Username, string Password) : ICommand<Account>;

internal class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username không được để trống");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password không được để trống")
            .MaximumLength(256);
    }
}

internal class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, Account>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Account>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var existingAccount = await _accountRepository.FindSingleAsync(a => a.UserName == request.Username);
        if (existingAccount != null)
        {
            return Result.Failure<Account>(new Error("400", "Username đã tồn tại"));
        }

        var account = Account.Create(request.Username, request.Password);

        await _accountRepository.AddAsync(account);

        await _unitOfWork.CommitAsync();

        return Result.Success(account);    
    }
}