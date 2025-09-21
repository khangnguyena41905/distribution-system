namespace IDENTITY.DOMAIN.Abstractions.Entities;

public abstract class AuditEntity<TKey> : DomainEntity<TKey>
{
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedDate { get; set; }
    public string? DeletedBy { get; set; }
}