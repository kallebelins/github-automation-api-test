using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GithubAutomationApiTest.Application.DTOs;

namespace GithubAutomationApiTest.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync(int page, int size, string? category, CancellationToken cancellationToken);
    }
}