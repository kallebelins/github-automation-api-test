using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GithubAutomationApiTest.Application.Interfaces;
using GithubAutomationApiTest.Domain.Entities;

namespace GithubAutomationApiTest.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        public ProductRepository()
        {
            _products = new List<Product>();
        }

        public Task<(IEnumerable<Product> Items, int TotalItems)> GetActiveProductsAsync(string? category, int page, int size, CancellationToken cancellationToken)
        {
            var query = _products.AsQueryable().Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            var totalItems = query.Count();
            var items = query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            return Task.FromResult((items.AsEnumerable(), totalItems));
        }
    }
}