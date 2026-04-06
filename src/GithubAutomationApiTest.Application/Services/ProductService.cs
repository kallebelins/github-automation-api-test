using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GithubAutomationApiTest.Application.DTOs;
using GithubAutomationApiTest.Application.Interfaces;
using GithubAutomationApiTest.Domain.Entities;

namespace GithubAutomationApiTest.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<PagedResultDto<ProductDto>> GetProductsAsync(string? category, int page, int size, CancellationToken cancellationToken)
        {
            if (page < 1)
            {
                throw new ArgumentException("Page must be greater than or equal to 1.", nameof(page));
            }

            if (size < 1 || size > 100)
            {
                throw new ArgumentException("Size must be between 1 and 100.", nameof(size));
            }

            var (items, totalItems) = await _productRepository.GetActiveProductsAsync(category, page, size, cancellationToken);

            var productDtos = items.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Price = product.Price
            }).ToList();

            var totalPages = (int)Math.Ceiling(totalItems / (double)size);

            return new PagedResultDto<ProductDto>
            {
                Items = productDtos,
                Page = page,
                Size = size,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }
    }
}