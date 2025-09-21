using IDENTITY.DOMAIN.Abstractions.Entities;
using IDENTITY.DOMAIN.Entities.Identities;
using Microsoft.AspNetCore.Identity;

namespace IDENTITY.DOMAIN.Entities.Accounts;

public class Account : DomainEntity<Guid>
{
    public string UserName { get; private set; }
    public string Password { get; private set; }
    
    // public virtual AppUser AppUser { get; set; }
    
    public Account(){}

    public static Account Create(string userName, string password)
    {
        var hasher = new PasswordHasher<Account>();
        var account = new Account
        {
            Id = Guid.NewGuid(),
            UserName = userName
        };
        account.Password = hasher.HashPassword(account, password);
        return account;
    }


    public void Update(string password)
    {
        this.Password = password;
    }
}