using System.Net;
using System.Net.Http.Json;

namespace Products.IntegrationTests.Health;

public class HealthEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HealthEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHealth_ReturnsOk()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<HealthResponse>();

        Assert.NotNull(content);
        Assert.Equal("Healthy", content.Status);
        Assert.Equal("Available", content.Database);
    }

    private sealed record HealthResponse(string Status, string Database);
}
