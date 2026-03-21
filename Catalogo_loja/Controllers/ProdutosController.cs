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

    // 1. GET: api/produtos (Lista com filtros de Nome e Categoria)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos([FromQuery] string? nome, [FromQuery] string? categoria)
    {
        var produtos = await _service.GetAllAsync(nome, categoria);
        return Ok(produtos);
    }

    // 2. GET: api/produtos/categorias (NOVO MÉTODO)
    [HttpGet("categorias")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategorias()
    {
        var categorias = await _service.GetCategoriasAsync();
        return Ok(categorias);
    }

    // 3. GET: api/produtos/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> GetProduto(Guid id)
    {
        var produto = await _service.GetByIdAsync(id);

        if (produto == null)
            return NotFound(new { mensagem = "Produto não encontrado." });

        return Ok(produto);
    }

    // 4. POST: api/produtos
    [HttpPost]
    public async Task<ActionResult<Produto>> PostProduto(ProdutoDto dto)
    {
        var produtoCriado = await _service.CreateAsync(dto);

        // Retorna o status 201 e o link para consultar o produto recém-criado
        return CreatedAtAction(nameof(GetProduto), new { id = produtoCriado.Id }, produtoCriado);
    }

    // 5. PUT: api/produtos/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduto(Guid id, [FromBody] ProdutoDto dto)
    {
        var sucesso = await _service.UpdateAsync(id, dto);

        if (!sucesso)
            return NotFound(new { mensagem = "Não foi possível atualizar. Produto não encontrado." });

        return NoContent(); // Sucesso sem retorno de conteúdo (Padrão REST)
    }

    // 6. DELETE: api/produtos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(Guid id)
    {
        var sucesso = await _service.DeleteAsync(id);

        if (!sucesso)
            return NotFound(new { mensagem = "Produto não encontrado para exclusão." });

        return NoContent();
    }
}