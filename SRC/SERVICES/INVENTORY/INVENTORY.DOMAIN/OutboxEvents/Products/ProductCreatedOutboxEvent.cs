using COMMON.CONTRACT.Abstractions.Message;

namespace INVENTORY.DOMAIN.OutboxEvents.Products;

public class ProductCreatedOutboxEvent : IOutboxEvent
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
}