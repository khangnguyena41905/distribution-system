namespace INVENTORY.APPLICATION.Requests;

public record CreateProductRequest(
    string Name,
    string Description,
    decimal Price
);

public record UpdateProductRequest(
    string Name,
    string Description,
    decimal Price
);

