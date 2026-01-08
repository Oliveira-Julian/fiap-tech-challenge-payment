using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Application.Pedidos;

public interface IPedidoGateway
{
    Task<Pedido> ObterPedidoComRelacionamentosAsync(Guid idPedido, CancellationToken cancellationToken, bool tracking = false);
    Task<Pedido> ObterPedidoAsync(Guid idPedido, CancellationToken cancellationToken, bool tracking = false);
    void AtualizarPedido(Pedido pedido);
    Task<IEnumerable<Pedido>> ObterPedidosPorProdutoAsync(Guid idProduto, IEnumerable<PedidoStatus> pedidosStatus, CancellationToken cancellationToken, bool tracking = false);
}
