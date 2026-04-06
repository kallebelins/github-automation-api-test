using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GithubAutomationApiTest.Domain.Entities;

namespace GithubAutomationApiTest.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<(IEnumerable<Product> Items, int TotalItems)> GetActiveProductsAsync(string? category, int page, int size, CancellationToken cancellationToken);
    }
}