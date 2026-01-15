using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Application.Pedidos;

public interface IPedidoGateway
{
    Task<Pedido> ObterPedidoAsync(Guid idPedido, CancellationToken cancellationToken);
    Task ConfirmarPagamentoAsync(Guid idPedido, CancellationToken cancellationToken);
}
