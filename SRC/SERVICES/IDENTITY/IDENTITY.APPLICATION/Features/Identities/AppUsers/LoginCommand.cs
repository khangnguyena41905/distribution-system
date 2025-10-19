using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IDENTITY.APPLICATION.Dtos.Users;
using IDENTITY.APPLICATION.Features.Caching;
using IDENTITY.APPLICATION.Features.Identities.Permissions;
using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public record LoginCommand(string UserName, string Password) : ICommand<LoginResultDto>;

internal class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResultDto>
{
    private readonly IAppUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;

    public LoginCommandHandler(
        IAppUserRepository userRepository,
        IConfiguration configuration,
        IMediator mediator)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _mediator = mediator;
    }

    public async Task<Result<LoginResultDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        
        var account = await _userRepository.FindSingleAsync(x=> x.UserName == request.UserName);
        var (user, roles) = await _userRepository.GetUserWithRolesAsync(account.Id);

        if (user == null)
            return Result.Failure<LoginResultDto>(new Error("401", "Invalid username or password"));
        
        if (!account.IsValid(request.Password))
        {
            return Result.Failure<LoginResultDto>(new Error("401", "Invalid username or password"));
        }

        // Tạo token JWT
        var token = GenerateJwtToken(user, roles.Select(r => r.Name).ToList(), out DateTime expiration);

        // Lấy permissions qua mediator (CQRS)
        var permissionsResult = await _mediator.Send(new GetUserPermissionFunctionsQuery(user.Id), cancellationToken);
        if (permissionsResult.IsFailure)
            return Result.Failure<LoginResultDto>(new Error("500", "Failed to load permissions"));

        // Cache permissions vào Redis
        await _mediator.Send(new SetUserPermissionsCommand(user.Id, permissionsResult.Value), cancellationToken);

        return Result.Success(new LoginResultDto
        {
            Token = token,
            Expiration = expiration,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!
        });
    }

    private string GenerateJwtToken(AppUser user, IList<string> roles, out DateTime expires)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email ?? "")
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        expires = DateTime.UtcNow.AddHours(2);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
