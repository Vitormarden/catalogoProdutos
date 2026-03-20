using System.ComponentModel.DataAnnotations;

namespace Catalogo_loja.DTOs;

public class ProdutoDto
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Preco { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Estoque { get; set; }

    [Required]
    public string Categoria { get; set; } = string.Empty;

    public string? ImagemUrl { get; set; }

    public bool Ativo { get; set; } = true;
}