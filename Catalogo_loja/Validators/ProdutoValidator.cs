using FluentValidation;
using Catalogo_loja.DTOs;

namespace Catalogo_loja.Validators;

public class ProdutoValidator : AbstractValidator<ProdutoDto>
{
    public ProdutoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres");

        RuleFor(x => x.Preco)
            .GreaterThan(0).WithMessage("O preço deve ser maior que zero");

        RuleFor(x => x.Estoque)
            .GreaterThanOrEqualTo(0).WithMessage("O estoque não pode ser negativo");

        RuleFor(x => x.Categoria)
            .NotEmpty().WithMessage("A categoria é obrigatória");
    }
}