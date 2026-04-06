using System.Threading;
using System.Threading.Tasks;
using GithubAutomationApiTest.Application.DTOs;

namespace GithubAutomationApiTest.Application.Interfaces
{
    public interface IProductService
    {
        Task<PagedResultDto<ProductDto>> GetProductsAsync(string? category, int page, int size, CancellationToken cancellationToken);
    }
}