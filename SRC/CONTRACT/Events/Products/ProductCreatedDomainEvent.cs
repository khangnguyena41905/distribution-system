using COMMON.CONTRACT.Abstractions.Message;

namespace CONTRACT.Events.Products;

public class ProductCreatedDomainEvent : IDomainEvent
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }

    public ProductCreatedDomainEvent(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}
