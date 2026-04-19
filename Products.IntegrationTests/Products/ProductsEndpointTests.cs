using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Products.Application.DTOs.Auth;
using Products.Application.DTOs.Products;

namespace Products.IntegrationTests.Products;

public class ProductsEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProductsEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/api/products");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PostProducts_WithoutToken_ReturnsUnauthorized()
    {
        var requestBody = new
        {
            name = "Gaming Mouse",
            description = "Wireless mouse",
            colour = "Black",
            price = 49.99m
        };

        var response = await _client.PostAsJsonAsync("/api/products", requestBody);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    [Fact]
public async Task GetProducts_WithValidToken_ReturnsOk()
{
    var token = await GetAccessTokenAsync();

    _client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);

    var response = await _client.GetAsync("/api/products");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
}

[Fact]
public async Task PostProducts_WithValidToken_ReturnsCreated()
{
    var token = await GetAccessTokenAsync();

    _client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);

    var request = new CreateProductRequest
    {
        Name = "Laptop Stand",
        Description = "Adjustable aluminum stand",
        Colour = "Silver",
        Price = 34.99m
    };

    var response = await _client.PostAsJsonAsync("/api/products", request);

    Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    var createdProduct = await response.Content.ReadFromJsonAsync<ProductResponse>();

    Assert.NotNull(createdProduct);
    Assert.Equal("Laptop Stand", createdProduct.Name);
    Assert.Equal("Silver", createdProduct.Colour);
}

[Fact]
public async Task GetProducts_ByColour_WithValidToken_ReturnsMatchingProducts()
{
    var token = await GetAccessTokenAsync();

    _client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);

    await _client.PostAsJsonAsync("/api/products", new CreateProductRequest
    {
        Name = "Mechanical Keyboard",
        Description = "RGB keyboard",
        Colour = "Red",
        Price = 89.99m
    });

    await _client.PostAsJsonAsync("/api/products", new CreateProductRequest
    {
        Name = "USB Headset",
        Description = "Noise cancelling headset",
        Colour = "Blue",
        Price = 59.99m
    });

    var response = await _client.GetAsync("/api/products?colour=red");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var products = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();

    Assert.NotNull(products);
    Assert.Contains(products, p => p.Colour.Equals("Red", StringComparison.OrdinalIgnoreCase));
    Assert.DoesNotContain(products, p => p.Colour.Equals("Blue", StringComparison.OrdinalIgnoreCase));
}

private async Task<string> GetAccessTokenAsync()
{
    var loginRequest = new LoginRequest
    {
        Username = "admin",
        Password = "Password123!"
    };

    var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
    loginResponse.EnsureSuccessStatusCode();

    var content = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

    Assert.NotNull(content);
    Assert.False(string.IsNullOrWhiteSpace(content.Token));

    return content.Token;
}
}