using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> GetProductsAsync([FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string? category = null, CancellationToken cancellationToken = default)
        {
            if (page <= 0 || size <= 0)
            {
                return BadRequest("Page and size must be greater than zero.");
            }

            var products = await _productService.GetProductsAsync(page, size, category, cancellationToken);
            return Ok(products);
        }
    }
}