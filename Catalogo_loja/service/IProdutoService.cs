using Catalogo_loja.Models;
using Catalogo_loja.DTOs;

namespace Catalogo_loja.Services;

public interface IProdutoService
{
    Task<(IEnumerable<Produto> Items, PaginationMetadata Metadata)> GetAllAsync(string? nome, string? categoria, int pageNumber, int pageSize);

    Task<Produto?> GetByIdAsync(Guid id);

    Task<Produto> CreateAsync(ProdutoDto dto);

    Task<bool> UpdateAsync(Guid id, ProdutoDto dto);

    Task<bool> DeleteAsync(Guid id);

    Task<IEnumerable<string>> GetCategoriasAsync();
}