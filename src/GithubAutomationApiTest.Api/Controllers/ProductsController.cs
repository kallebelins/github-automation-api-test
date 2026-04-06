using System;
using System.Threading;
using System.Threading.Tasks;
using GithubAutomationApiTest.Application.DTOs;
using GithubAutomationApiTest.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GithubAutomationApiTest.Api.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] int page = 1, [FromQuery] int size = 20, [FromQuery] string? category = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _productService.GetProductsAsync(category, page, size, cancellationToken);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Parâmetros inválidos", details = ex.Message });
            }
        }
    }
}