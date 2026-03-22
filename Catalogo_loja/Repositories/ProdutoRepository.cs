using Catalogo_loja.Data;
using Catalogo_loja.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalogo_loja.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> GetAllAsync(string? nome, string? categoria)
    {
        var query = _context.Produtos.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
            query = query.Where(x => x.Nome.ToLower().Contains(nome.ToLower()));

        if (!string.IsNullOrWhiteSpace(categoria))
            query = query.Where(x => x.Categoria == categoria);

        return await query.ToListAsync();
    }

    public async Task<Produto?> GetByIdAsync(Guid id)
        => await _context.Produtos.FindAsync(id);

    public async Task<Produto> AddAsync(Produto produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task<bool> UpdateAsync(Produto produto)
    {
        _context.Produtos.Update(produto);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null) return false;

        _context.Produtos.Remove(produto);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<string>> GetCategoriasAsync()
    {
        return await _context.Produtos
            .AsNoTracking()
            .Select(x => x.Categoria)
            .Distinct()
            .ToListAsync();
    }
}