using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Produtos;

namespace FoodChallenge.Payment.UnitTests.Produtos.Entities;

public class ProdutoTests : TestBase
{
    [Fact]
    public void Produto_DevePermitirDefinirPropriedades()
    {
        var produto = new Produto
        {
            Id = Guid.NewGuid(),
            Nome = "Hambúrguer",
            Descricao = "Hambúrguer artesanal",
            Preco = 25.90m,
            Categoria = ProdutoCategoria.Lanche,
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };
        Assert.NotEqual(Guid.Empty, produto.Id);
        Assert.Equal("Hambúrguer", produto.Nome);
        Assert.Equal("Hambúrguer artesanal", produto.Descricao);
        Assert.Equal(25.90m, produto.Preco);
        Assert.Equal(ProdutoCategoria.Lanche, produto.Categoria);
        Assert.True(produto.Ativo);
        Assert.True(produto.DataCriacao <= DateTime.UtcNow);
    }
}
