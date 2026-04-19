namespace Products.Application.DTOs.Products;

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Colour { get; set; } = string.Empty;

    public decimal Price { get; set; }
}