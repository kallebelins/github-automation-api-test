using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GithubAutomationApiTest.Domain.Entities;
using GithubAutomationApiTest.Domain.Interfaces;

namespace GithubAutomationApiTest.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new()
        {
            new Product { Id = Guid.NewGuid(), Name = "Product A", Category = "Category 1", Price = 10.0m, IsActive = true },
            new Product { Id = Guid.NewGuid(), Name = "Product B", Category = "Category 2", Price = 20.0m, IsActive = true },
            new Product { Id = Guid.NewGuid(), Name = "Product C", Category = "Category 1", Price = 15.0m, IsActive = false }
        };

        public Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_products.Where(p => p.IsActive));
        }
    }
}