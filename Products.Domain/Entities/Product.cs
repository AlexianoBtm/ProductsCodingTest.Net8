namespace Products.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Colour { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}