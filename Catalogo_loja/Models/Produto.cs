using System.ComponentModel.DataAnnotations;

namespace Catalogo_loja.Models;

public class Produto
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "O preço é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "O estoque é obrigatório")]
    [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo")]
    public int Estoque { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória")]
    public string Categoria { get; set; } = string.Empty;

    public string? ImagemUrl { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
}