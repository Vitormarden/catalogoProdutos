using Catalogo_loja.Models;
using Catalogo_loja.DTOs;
using AutoMapper;
using Catalogo_loja.Repositories;

namespace Catalogo_loja.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _repository;
    private readonly IMapper _mapper;

    public ProdutoService(IProdutoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<(IEnumerable<Produto> Items, PaginationMetadata Metadata)> GetAllAsync(string? nome, string? categoria, int pageNumber, int pageSize)
        => await _repository.GetAllAsync(nome, categoria, pageNumber, pageSize);

    public async Task<Produto?> GetByIdAsync(Guid id)
        => await _repository.GetByIdAsync(id);

    public async Task<Produto> CreateAsync(ProdutoDto dto)
    {
        var produto = _mapper.Map<Produto>(dto);
        return await _repository.AddAsync(produto);
    }

    public async Task<bool> UpdateAsync(Guid id, ProdutoDto dto)
    {
        var produto = await _repository.GetByIdAsync(id);
        if (produto == null) return false;

        _mapper.Map(dto, produto);
        return await _repository.UpdateAsync(produto);
    }

    public async Task<bool> DeleteAsync(Guid id)
        => await _repository.DeleteAsync(id);

    public async Task<IEnumerable<string>> GetCategoriasAsync()
        => await _repository.GetCategoriasAsync();
}