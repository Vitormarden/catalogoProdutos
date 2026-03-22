using Catalogo_loja.Models;

namespace Catalogo_loja.Repositories;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> GetAllAsync(string? nome, string? categoria);
    Task<Produto?> GetByIdAsync(Guid id);
    Task<Produto> AddAsync(Produto produto);
    Task<bool> UpdateAsync(Produto produto);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<string>> GetCategoriasAsync();
}