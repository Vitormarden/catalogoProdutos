using AutoMapper;
using Catalogo_loja.DTOs;
using Catalogo_loja.Models;
using Catalogo_loja.Repositories;
using Catalogo_loja.Services;
using Catalogo_loja.Mappings;
using FluentAssertions;
using Moq;
using Xunit;

namespace Catalogo_loja.Tests;

public class ProdutoServiceTests
{
    private readonly Mock<IProdutoRepository> _repositoryMock;
    private readonly IMapper _mapper;
    private readonly ProdutoService _service;

    public ProdutoServiceTests()
    {
        _repositoryMock = new Mock<IProdutoRepository>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _service = new ProdutoService(_repositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarProduto_QuandoIdExistir()
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        var produtoFake = new Produto { Id = produtoId, Nome = "Produto Fake", Ativo = true };
        _repositoryMock.Setup(r => r.GetByIdAsync(produtoId)).ReturnsAsync(produtoFake);

        // Act
        var result = await _service.GetByIdAsync(produtoId);

        // Assert
        result.Should().NotBeNull();
        result!.Nome.Should().Be("Produto Fake");
        _repositoryMock.Verify(r => r.GetByIdAsync(produtoId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DeveChamarAddAsync_E_RetornarProduto()
    {
        // Arrange
        var dto = new ProdutoDto { Nome = "Novo Produto", Preco = 50, Categoria = "Geral" };
        var produtoMapeado = _mapper.Map<Produto>(dto);
        
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Produto>()))
                       .ReturnsAsync((Produto p) => p);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Nome.Should().Be(dto.Nome);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Produto>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_DeveRetornarFalse_QuandoProdutoNaoExistir()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new ProdutoDto { Nome = "Update" };
        _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Produto?)null);

        // Act
        var result = await _service.UpdateAsync(id, dto);

        // Assert
        result.Should().BeFalse();
    }
}