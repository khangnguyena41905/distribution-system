using IDENTITY.DOMAIN.Entities.Accounts;
using Microsoft.AspNetCore.Identity;

namespace IDENTITY.DOMAIN.Entities.Identities;


public class AppUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public DateTime? DayOfBirth { get; set; }
    public bool? IsDirector { get; set; }
    public bool? IsHeadOfDepartment { get; set; }
    public Guid? ManagerId { get; set; }
    public Guid PositionId { get; set; }
    public int IsReceipient {  get; set; }
    public Guid AccountId { get; set; }  
    public virtual Account Account { get; set; }  
    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }
    public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }
    public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; }
    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
    
    public static AppUser Create(
        string firstName,
        string lastName,
        DateTime? dayOfBirth,
        bool? isDirector,
        bool? isHeadOfDepartment,
        Guid? managerId,
        Guid positionId,
        int isReceipient,
        Guid accountId,
        string userName,
        string email)
    {
        return new AppUser
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            FullName = $"{lastName} {firstName}".Trim(),
            DayOfBirth = dayOfBirth,
            IsDirector = isDirector,
            IsHeadOfDepartment = isHeadOfDepartment,
            ManagerId = managerId,
            PositionId = positionId,
            IsReceipient = isReceipient,
            AccountId = accountId,
            UserName = userName,
            NormalizedUserName = userName?.ToUpper(),
            Email = email,
            NormalizedEmail = email?.ToUpper(),
            EmailConfirmed = false,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
    }

    public void Update(
        string firstName,
        string lastName,
        DateTime? dayOfBirth,
        bool? isDirector,
        bool? isHeadOfDepartment,
        Guid? managerId,
        Guid positionId,
        int isReceipient,
        string? userName,
        string? email)
    {
        FirstName = firstName;
        LastName = lastName;
        FullName = $"{lastName} {firstName}".Trim();
        DayOfBirth = dayOfBirth;
        IsDirector = isDirector;
        IsHeadOfDepartment = isHeadOfDepartment;
        ManagerId = managerId;
        PositionId = positionId;
        IsReceipient = isReceipient;

        if (!string.IsNullOrWhiteSpace(userName))
        {
            UserName = userName;
            NormalizedUserName = userName.ToUpper();
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            Email = email;
            NormalizedEmail = email.ToUpper();
        }

        ConcurrencyStamp = Guid.NewGuid().ToString(); // optional
    }

}