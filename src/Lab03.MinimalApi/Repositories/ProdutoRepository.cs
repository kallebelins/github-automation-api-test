using Lab03.MinimalApi.Data;
using Lab03.MinimalApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lab03.MinimalApi.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Produtos.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Produto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Produtos.FindAsync([id], cancellationToken);
    }

    public async Task<Produto> AddAsync(Produto produto, CancellationToken cancellationToken = default)
    {
        await _context.Produtos.AddAsync(produto, cancellationToken);
        return produto;
    }

    public Produto Update(Produto produto)
    {
        _context.Produtos.Update(produto);
        return produto;
    }

    public void Delete(Produto produto)
    {
        _context.Produtos.Remove(produto);
    }
}
