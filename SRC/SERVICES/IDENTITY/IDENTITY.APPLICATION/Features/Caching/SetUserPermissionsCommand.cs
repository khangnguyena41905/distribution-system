using FluentValidation;
using IDENTITY.APPLICATION.Abstractions;
using IDENTITY.APPLICATION.Dtos.PermissionDtos;
using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;

namespace IDENTITY.APPLICATION.Features.Caching;

public record SetUserPermissionsCommand(Guid UserId, List<FunctionDto> Permissions) : ICommand<bool>;

public class SetUserPermissionsCommandValidator : AbstractValidator<SetUserPermissionsCommand>
{
    public SetUserPermissionsCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Permissions).NotNull();
    }
}

internal class SetUserPermissionsCommandHandler : ICommandHandler<SetUserPermissionsCommand, bool>
{
    private readonly ICacheService _cacheService;

    private const string CachePrefix = "permissions:"; // hoặc move vào config

    public SetUserPermissionsCommandHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<Result<bool>> Handle(SetUserPermissionsCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{CachePrefix}{request.UserId}";

        await _cacheService.SetAsync(cacheKey, request.Permissions, TimeSpan.FromHours(1), cancellationToken);

        return Result.Success(true);
    }
}