using Products.Application.Interfaces;
using Products.Domain.Entities;

namespace Products.UnitTests.TestDoubles;

public class FakeProductRepository : IProductRepository
{
    private readonly List<Product> _products = new();

    public Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        _products.Add(product);
        return Task.FromResult(product);
    }

    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult((IReadOnlyList<Product>)_products.ToList());
    }

    public Task<IReadOnlyList<Product>> GetByColourAsync(string colour, CancellationToken cancellationToken = default)
    {
        var filteredProducts = _products
            .Where(p => string.Equals(p.Colour, colour, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Task.FromResult((IReadOnlyList<Product>)filteredProducts);
    }
}