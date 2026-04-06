using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GithubAutomationApiTest.Application.DTOs;
using GithubAutomationApiTest.Application.Interfaces;
using GithubAutomationApiTest.Domain.Entities;
using GithubAutomationApiTest.Domain.Interfaces;

namespace GithubAutomationApiTest.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(int page, int size, string? category, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetActiveProductsAsync(cancellationToken);

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            return products
                .OrderBy(p => p.Name)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Price = p.Price
                });
        }
    }
}