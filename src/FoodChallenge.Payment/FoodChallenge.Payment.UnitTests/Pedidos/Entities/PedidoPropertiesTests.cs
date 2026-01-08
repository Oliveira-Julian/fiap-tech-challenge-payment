using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.UnitTests.Pedidos.Entities;

public class PedidoPropertiesTests : TestBase
{
    [Fact]
    public void Pedido_DevePermitirDefinirTodasPropriedades()
    {
        var idCliente = Guid.NewGuid();
        var idPagamento = Guid.NewGuid();
        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            IdCliente = idCliente,
            IdPagamento = idPagamento,
            Codigo = "ABC123",
            ValorTotal = 150.75m,
            Status = PedidoStatus.Recebido,
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };
        Assert.NotEqual(Guid.Empty, pedido.Id);
        Assert.Equal(idCliente, pedido.IdCliente);
        Assert.Equal(idPagamento, pedido.IdPagamento);
        Assert.Equal("ABC123", pedido.Codigo);
        Assert.Equal(150.75m, pedido.ValorTotal);
        Assert.Equal(PedidoStatus.Recebido, pedido.Status);
        Assert.True(pedido.Ativo);
        Assert.True(pedido.DataCriacao <= DateTime.UtcNow);
    }
}
