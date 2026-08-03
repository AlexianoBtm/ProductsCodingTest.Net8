using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Infrastructure.Persistence;

namespace Products.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    private readonly ProductsDbContext _dbContext;
    private readonly ILogger<HealthController> _logger;

    public HealthController(
        ProductsDbContext dbContext,
        ILogger<HealthController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);

            if (canConnect)
            {
                return Ok(new { status = "Healthy", database = "Available" });
            }
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Database health check failed.");
        }

        return StatusCode(
            StatusCodes.Status503ServiceUnavailable,
            new { status = "Unhealthy", database = "Unavailable" });
    }
}
