using System.Net;
using System.Net.Http.Json;
using Products.Application.DTOs.Auth;

namespace Products.IntegrationTests.Auth;

public class AuthEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOkAndToken()
    {
        var request = new LoginRequest
        {
            Username = "admin",
            Password = "Password123!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(content);
        Assert.False(string.IsNullOrWhiteSpace(content.Token));
        Assert.True(content.ExpiresAtUtc > DateTime.UtcNow);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        var request = new LoginRequest
        {
            Username = "admin",
            Password = "wrong"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}