using Products.Application.DTOs.Products;
using Products.Application.Services;
using Products.UnitTests.TestDoubles;

namespace Products.UnitTests.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task CreateAsync_WithValidRequest_ReturnsCreatedProduct()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var request = new CreateProductRequest
        {
            Name = "Gaming Mouse",
            Description = "Wireless mouse",
            Colour = "Black",
            Price = 49.99m
        };

        var result = await service.CreateAsync(request);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Gaming Mouse", result.Name);
        Assert.Equal("Wireless mouse", result.Description);
        Assert.Equal("Black", result.Colour);
        Assert.Equal(49.99m, result.Price);
        Assert.True(result.CreatedAtUtc <= DateTime.UtcNow);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyName_ThrowsArgumentException()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var request = new CreateProductRequest
        {
            Name = "",
            Description = "Wireless mouse",
            Colour = "Black",
            Price = 49.99m
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(request));

        Assert.Contains("name", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyColour_ThrowsArgumentException()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var request = new CreateProductRequest
        {
            Name = "Gaming Mouse",
            Description = "Wireless mouse",
            Colour = "",
            Price = 49.99m
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(request));

        Assert.Contains("colour", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidPrice_ThrowsArgumentException()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var request = new CreateProductRequest
        {
            Name = "Gaming Mouse",
            Description = "Wireless mouse",
            Colour = "Black",
            Price = 0m
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(request));

        Assert.Contains("price", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllProducts()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        await service.CreateAsync(new CreateProductRequest
        {
            Name = "Gaming Mouse",
            Description = "Wireless mouse",
            Colour = "Black",
            Price = 49.99m
        });

        await service.CreateAsync(new CreateProductRequest
        {
            Name = "Mechanical Keyboard",
            Description = "RGB keyboard",
            Colour = "Red",
            Price = 89.99m
        });

        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByColourAsync_ReturnsMatchingProducts()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        await service.CreateAsync(new CreateProductRequest
        {
            Name = "Gaming Mouse",
            Description = "Wireless mouse",
            Colour = "Black",
            Price = 49.99m
        });

        await service.CreateAsync(new CreateProductRequest
        {
            Name = "Mechanical Keyboard",
            Description = "RGB keyboard",
            Colour = "Red",
            Price = 89.99m
        });

        var result = await service.GetByColourAsync("Black");

        Assert.Single(result);
        Assert.Equal("Gaming Mouse", result[0].Name);
    }

    [Fact]
    public async Task GetByColourAsync_IsCaseInsensitive()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        await service.CreateAsync(new CreateProductRequest
        {
            Name = "Gaming Mouse",
            Description = "Wireless mouse",
            Colour = "Black",
            Price = 49.99m
        });

        var result = await service.GetByColourAsync("black");

        Assert.Single(result);
        Assert.Equal("Black", result[0].Colour);
    }
}