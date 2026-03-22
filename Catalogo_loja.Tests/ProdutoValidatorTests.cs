using Catalogo_loja.DTOs;
using Catalogo_loja.Validators;
using FluentAssertions;
using Xunit;

namespace Catalogo_loja.Tests;

public class ProdutoValidatorTests
{
    private readonly ProdutoValidator _validator;

    public ProdutoValidatorTests()
    {
        _validator = new ProdutoValidator();
    }

    [Fact]
    public void DeveRetornarErroQuandoNomeForVazio()
    {
        // Arrange
        var dto = new ProdutoDto { Nome = "", Preco = 10, Categoria = "Teste" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Nome");
    }

    [Fact]
    public void DeveRetornarErroQuandoPrecoForMenorOuIgualAZero()
    {
        // Arrange
        var dto = new ProdutoDto { Nome = "Produto Teste", Preco = 0, Categoria = "Teste" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Preco");
    }

    [Fact]
    public void DeveSerValidoQuandoTodosOsCamposEstiveremCorretos()
    {
        // Arrange
        var dto = new ProdutoDto 
        { 
            Nome = "Produto Valido", 
            Preco = 100.50m, 
            Categoria = "Eletrônicos",
            Estoque = 10
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}