using INVENTORY.DOMAIN.Entities;
using Microsoft.EntityFrameworkCore;

namespace INVENTORY.PERSISTENCE.Abstractions.DbContexts;
public sealed class ApplicationDbContext : DbContext
{
    private readonly AuditInterceptor _auditInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        AuditInterceptor? auditInterceptor = null) 
        : base(options)
    {
        _auditInterceptor = auditInterceptor;
    }

    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_auditInterceptor != null)
        {
            optionsBuilder.AddInterceptors(_auditInterceptor);
        }
    }
    
    public DbSet<Product> AppUsers { get; set; }
}
