using Catalogo_loja.Data;
using Catalogo_loja.Models;
using Catalogo_loja.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Catalogo_loja.Services;

public class ProdutoService : IProdutoService
{
    private readonly AppDbContext _context;

    public ProdutoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> GetAllAsync(string? nome, string? categoria)
    {
        //  AsNoTracking para performance (leitura otimizada)
        var query = _context.Produtos.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
        {
            // ignora maiúsculas/minúsculas
            query = query.Where(x => x.Nome.ToLower().Contains(nome.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(categoria))
            query = query.Where(x => x.Categoria == categoria);

        return await query.ToListAsync();
    }

    public async Task<Produto?> GetByIdAsync(Guid id)
        => await _context.Produtos.FindAsync(id);

    public async Task<Produto> CreateAsync(ProdutoDto dto)
    {
        var produto = new Produto
        {
            // O ID é gerado automaticamente no construtor da Model Produto
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            Estoque = dto.Estoque,
            Categoria = dto.Categoria,
            ImagemUrl = dto.ImagemUrl,
            Ativo = dto.Ativo
        };

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task<bool> UpdateAsync(Guid id, ProdutoDto dto)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto == null) return false;

        produto.Nome = dto.Nome;
        produto.Descricao = dto.Descricao;
        produto.Preco = dto.Preco;
        produto.Estoque = dto.Estoque;
        produto.Categoria = dto.Categoria;
        produto.ImagemUrl = dto.ImagemUrl;
        produto.Ativo = dto.Ativo;

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