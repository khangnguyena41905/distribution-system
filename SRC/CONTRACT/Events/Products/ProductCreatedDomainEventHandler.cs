using COMMON.CONTRACT.Abstractions.Message;

namespace CONTRACT.Events.Products;

public class ProductCreatedDomainEventHandler : IDomainHandler<ProductCreatedDomainEvent>
{
    public Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Product created: {notification.Id} - {notification.Name} - {notification.Price}");
        return Task.CompletedTask;
    }
}