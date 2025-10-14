namespace COMMON.CONTRACT.Abstractions.Shared;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
}
