using FluentValidation;
using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN.Abstractions.Repositories.Accounts;
using IDENTITY.DOMAIN.Entities.Accounts;

namespace IDENTITY.APPLICATION.Features.Accounts;

public record CheckAccountQuery(string username, string password) : IQuery<Account>;

    
internal class CheckAccountQueryValidator : AbstractValidator<CheckAccountQuery>
{
    public CheckAccountQueryValidator()
    {
        RuleFor(x => x.username)
            .NotEmpty().WithMessage("username không được để trống");

        RuleFor(x => x.password)
            .NotEmpty().WithMessage("password không được để trống")
            .MaximumLength(256);
    }
}

internal class CheckAccountQueryHandler : IQueryHandler<CheckAccountQuery, Account>
{
    private readonly IAccountRepository _accountRepository;

    public CheckAccountQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Result<Account>> Handle(CheckAccountQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.FindSingleAsync(x => x.UserName == request.username);

        if (account is null)
            return Result.Failure<Account>(new Error("400","Tài khoản không tồn tại"));

        if (!account.Password.Equals(request.password))
            return Result.Failure<Account>(new Error("400","Sai mật khẩu"));

        // Nếu dùng mã hoá mật khẩu, thì kiểm tra như sau (giả sử dùng BCrypt hoặc tương tự)
        // if (!PasswordHasher.Verify(account.Password, request.password))
        //     return Result<Account>.Failure("Sai mật khẩu");

        return Result<Account>.Success(account);
    }
}
