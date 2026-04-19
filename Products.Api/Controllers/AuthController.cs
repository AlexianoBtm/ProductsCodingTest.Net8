using Microsoft.AspNetCore.Mvc;
using Products.Application.DTOs.Auth;
using Products.Application.Interfaces;

namespace Products.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request is null)
        {
            return BadRequest(new { error = "Request body is required." });
        }

        if (request.Username != "admin" || request.Password != "Password123!")
        {
            return Unauthorized(new { error = "Invalid username or password." });
        }

        var response = _tokenService.CreateToken(request.Username);

        return Ok(response);
    }
}