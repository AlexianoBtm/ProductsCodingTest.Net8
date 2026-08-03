using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Infrastructure.Persistence;

namespace Products.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string TestUsername = "integration-demo-user";
    public const string TestPassword = "integration-demo-password-only";

    private const string TestJwtKey =
        "integration-test-jwt-signing-key-not-for-any-other-use";

    private readonly string _testDirectory = Path.Combine(
        Path.GetTempPath(),
        $"products-api-tests-{Guid.NewGuid():N}");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Directory.CreateDirectory(_testDirectory);
        var databasePath = Path.Combine(_testDirectory, "products-tests.db");

        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = TestJwtKey,
                ["Jwt:Issuer"] = "ProductsApi.IntegrationTests",
                ["Jwt:Audience"] = "ProductsApi.IntegrationTests.Client",
                ["Jwt:ExpiryMinutes"] = "5",
                ["DemoAuth:Username"] = TestUsername,
                ["DemoAuth:Password"] = TestPassword,
                ["ConnectionStrings:ProductsDb"] = $"Data Source={databasePath}",
                ["Frontend:AllowedOrigins:0"] = "http://localhost:5173"
            });
        });
    }

    public async Task ResetDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();

        await dbContext.Products.ExecuteDeleteAsync();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing && Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, recursive: true);
        }
    }
}
