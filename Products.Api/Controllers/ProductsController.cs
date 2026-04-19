using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Application.DTOs.Products;
using Products.Application.Interfaces;

namespace Products.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? colour, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(colour))
        {
            var filteredProducts = await _productService.GetByColourAsync(colour, cancellationToken);
            return Ok(filteredProducts);
        }

        var products = await _productService.GetAllAsync(cancellationToken);
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var createdProduct = await _productService.CreateAsync(request, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, createdProduct);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                error = ex.Message
            });
        }
    }
}