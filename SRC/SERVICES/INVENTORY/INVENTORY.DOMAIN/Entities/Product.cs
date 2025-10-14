using COMMON.CONTRACT.Abstractions.Message;
using CONTRACT.Events.Products;
using INVENTORY.DOMAIN.Abstractions.Aggregates;
using INVENTORY.DOMAIN.Abstractions.Entities;

namespace INVENTORY.DOMAIN.Entities;

public class Product : AuditEntity<Guid> 
{
    private Product(){}
    
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    public decimal Price { get; private set; }

    public static Product Create(string name, string description, decimal price)
    {
        var newEntity = new Product()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price
        };

        newEntity.RaiseDomainEvent(new ProductCreatedDomainEvent(newEntity.Id, name, price));

        return newEntity;
    }
}