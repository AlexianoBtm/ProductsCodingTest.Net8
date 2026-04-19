using Products.Application.DTOs.Products;
using Products.Application.Interfaces;
using Products.Domain.Entities;

namespace Products.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse> CreateAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateCreateRequest(request);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
            Colour = request.Colour.Trim(),
            Price = request.Price,
            CreatedAtUtc = DateTime.UtcNow
        };
        var savedProduct = await _productRepository.AddAsync(product, cancellationToken);

        return MapToResponse(savedProduct);

        
    }

    public async Task<IReadOnlyList<ProductResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);

        return products
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<IReadOnlyList<ProductResponse>> GetByColourAsync(
        string colour,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(colour))
        {
            return await GetAllAsync(cancellationToken);
        }

        var products = await _productRepository.GetByColourAsync(colour.Trim(), cancellationToken);

        return products
            .Select(MapToResponse)
            .ToList();
    }

    private static void ValidateCreateRequest(CreateProductRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Product name is required.", nameof(request.Name));

        if (string.IsNullOrWhiteSpace(request.Colour))
            throw new ArgumentException("Product colour is required.", nameof(request.Colour));

        if (request.Price <= 0)
            throw new ArgumentException("Product price must be greater than 0.", nameof(request.Price));
    }

    private static ProductResponse MapToResponse(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Colour = product.Colour,
            Price = product.Price,
            CreatedAtUtc = product.CreatedAtUtc
        };
    }
}