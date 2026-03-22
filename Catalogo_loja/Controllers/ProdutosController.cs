using Microsoft.AspNetCore.Mvc;
using Catalogo_loja.Models;
using Catalogo_loja.DTOs;
using Catalogo_loja.Services;

namespace Catalogo_loja.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _service;

    public ProdutosController(IProdutoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Lista os produtos com suporte a busca, filtros e paginação no servidor.
    /// </summary>
    /// <param name="nome">Filtro opcional por nome do produto.</param>
    /// <param name="categoria">Filtro opcional por categoria.</param>
    /// <param name="pageNumber">O número da página a ser retornada.</param>
    /// <param name="pageSize">A quantidade de itens por página.</param>
    /// <returns>Uma lista paginada de produtos ativos.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos(
        [FromQuery] string? nome, 
        [FromQuery] string? categoria,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var (produtos, metadata) = await _service.GetAllAsync(nome, categoria, pageNumber, pageSize);
        
        // Adiciona metadados de paginação no Header da resposta (Padrão REST maduro)
        Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metadata));

        return Ok(produtos);
    }

    /// <summary>
    /// Retorna a lista de todas as categorias únicas cadastradas.
    /// </summary>
    [HttpGet("categorias")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategorias()
    {
        var categorias = await _service.GetCategoriasAsync();
        return Ok(categorias);
    }

    /// <summary>
    /// Obtém os detalhes de um produto específico pelo ID.
    /// </summary>
    /// <param name="id">O identificador único do produto (Guid).</param>
    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> GetProduto(Guid id)
    {
        var produto = await _service.GetByIdAsync(id);

        if (produto == null)
            return NotFound(new { mensagem = "Produto não encontrado." });

        return Ok(produto);
    }

    /// <summary>
    /// Cria um novo produto no catálogo.
    /// </summary>
    /// <param name="dto">Dados do novo produto.</param>
    [HttpPost]
    public async Task<ActionResult<Produto>> PostProduto(ProdutoDto dto)
    {
        var produtoCriado = await _service.CreateAsync(dto);

        // Retorna o status 201 e o link para consultar o produto recém-criado
        return CreatedAtAction(nameof(GetProduto), new { id = produtoCriado.Id }, produtoCriado);
    }

    /// <summary>
    /// Atualiza os dados de um produto existente.
    /// </summary>
    /// <param name="id">O ID do produto a ser atualizado.</param>
    /// <param name="dto">Os novos dados do produto.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduto(Guid id, [FromBody] ProdutoDto dto)
    {
        var sucesso = await _service.UpdateAsync(id, dto);

        if (!sucesso)
            return NotFound(new { mensagem = "Não foi possível atualizar. Produto não encontrado." });

        return NoContent(); // Sucesso sem retorno de conteúdo (Padrão REST)
    }

    /// <summary>
    /// Realiza a remoção lógica (Soft Delete) de um produto.
    /// </summary>
    /// <param name="id">O ID do produto a ser removido.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(Guid id)
    {
        var sucesso = await _service.DeleteAsync(id);

        if (!sucesso)
            return NotFound(new { mensagem = "Produto não encontrado para exclusão." });

        return NoContent();
    }
}