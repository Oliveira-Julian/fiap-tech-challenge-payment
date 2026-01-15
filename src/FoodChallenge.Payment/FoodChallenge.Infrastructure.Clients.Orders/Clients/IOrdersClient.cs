using FoodChallenge.Infrastructure.Clients.Orders.Models;

namespace FoodChallenge.Infrastructure.Clients.Orders.Clients;

public interface IOrdersClient
{
    Task<Resposta<PedidoResponse>> ObterPedidoAsync(Guid idPedido, CancellationToken cancellationToken);
    Task<Resposta> ConfirmarPagamentoAsync(Guid idPedido, CancellationToken cancellationToken);
}
