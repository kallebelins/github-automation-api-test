using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GithubAutomationApiTest.Domain.Entities;

namespace GithubAutomationApiTest.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken);
    }
}