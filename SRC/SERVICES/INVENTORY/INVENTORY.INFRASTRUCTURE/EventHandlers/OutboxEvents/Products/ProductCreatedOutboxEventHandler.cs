using COMMON.CONTRACT.Abstractions.Message;
using INVENTORY.DOMAIN.OutboxEvents.Products;
using MassTransit;

namespace INVENTORY.INFRASTRUCTURE.EventHandlers.OutboxEvents.Products;

public class ProductCreatedOutboxEventHandler : IOutboxEventHandler<ProductCreatedOutboxEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedOutboxEvent> context)
    {
        Console.WriteLine("ProductCreatedOutboxEventHandler");
    }
}