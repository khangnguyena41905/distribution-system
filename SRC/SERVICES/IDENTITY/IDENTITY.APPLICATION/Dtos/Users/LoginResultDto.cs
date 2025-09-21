namespace IDENTITY.APPLICATION.Dtos.Users;

public class LoginResultDto
{
    public string Token { get; set; } = default!;
    public DateTime Expiration { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
}