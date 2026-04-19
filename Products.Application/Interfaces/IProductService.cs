using Products.Application.DTOs.Products;

namespace Products.Application.Interfaces;

public interface IProductService
{
    Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductResponse>> GetByColourAsync(string colour, CancellationToken cancellationToken = default);
}