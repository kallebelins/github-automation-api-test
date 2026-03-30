using Lab03.MinimalApi.Entities;

namespace Lab03.MinimalApi.Repositories;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Produto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Produto> AddAsync(Produto produto, CancellationToken cancellationToken = default);
    Produto Update(Produto produto);
    void Delete(Produto produto);
}
