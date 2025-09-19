using IDENTITY.DOMAIN.Abstractions.Entities;
using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.DOMAIN.Entities.Accounts;

public class Account : DomainEntity<Guid>
{
    public string UserName { get; private set; }
    public string Password { get; private set; }
    
    public virtual AppUser AppUser { get; set; }
    
    private Account(){}

    public static Account Create(string userName, string password)
    {
        return new Account()
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            Password = password
        };
    }

    public void Update(string password)
    {
        this.Password = password;
    }
}