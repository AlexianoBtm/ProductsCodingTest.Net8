using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Products.Application.Configuration;
using Products.Application.DTOs.Auth;
using Products.Application.Interfaces;

namespace Products.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly DemoAuthOptions _demoAuthOptions;

    public AuthController(
        ITokenService tokenService,
        IOptions<DemoAuthOptions> demoAuthOptions)
    {
        _tokenService = tokenService;
        _demoAuthOptions = demoAuthOptions.Value;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request is null)
        {
            return BadRequest(new { error = "Request body is required." });
        }

        if (!string.Equals(request.Username, _demoAuthOptions.Username, StringComparison.Ordinal) ||
            !string.Equals(request.Password, _demoAuthOptions.Password, StringComparison.Ordinal))
        {
            return Unauthorized(new { error = "Invalid username or password." });
        }

        var response = _tokenService.CreateToken(request.Username);

        return Ok(response);
    }
}
