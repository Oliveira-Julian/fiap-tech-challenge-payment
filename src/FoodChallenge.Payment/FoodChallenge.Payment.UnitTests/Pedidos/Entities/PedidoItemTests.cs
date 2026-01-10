using Bogus;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.UnitTests.Pedidos.Entities;

public class PedidoItemTests : TestBase
{
    private readonly Faker _faker;

    public PedidoItemTests()
    {
        _faker = GetFaker();
    }

    [Fact]
    public void Construtor_DeveCriarPedidoItem_ComIdECodigoGerado()
    {
        var pedidoItem = new PedidoItem();
        Assert.NotEqual(Guid.Empty, pedidoItem.Id);
        Assert.True(pedidoItem.Ativo);
        Assert.NotNull(pedidoItem.Codigo);
        Assert.Equal(6, pedidoItem.Codigo.Length);
        Assert.True(pedidoItem.DataCriacao <= DateTime.UtcNow);
    }

    [Fact]
    public void AtualizarValor_DeveRetornarProprioPedidoItem_ComValorAtualizado()
    {
        var pedidoItem = new PedidoItem();
        var novoValor = _faker.Random.Decimal(10, 100);
        var resultado = pedidoItem.AtualizarValor(novoValor);
        Assert.Same(pedidoItem, resultado);
        Assert.Equal(novoValor, pedidoItem.Valor);
    }
}
