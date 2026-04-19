using Products.Application.DTOs.Auth;

namespace Products.Application.Interfaces;

public interface ITokenService
{
    LoginResponse CreateToken(string username);
}