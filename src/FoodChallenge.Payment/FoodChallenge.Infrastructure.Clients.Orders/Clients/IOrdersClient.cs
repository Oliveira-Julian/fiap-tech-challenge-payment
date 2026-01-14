using FoodChallenge.Infrastructure.Clients.Orders.Models;

namespace FoodChallenge.Infrastructure.Clients.Orders.Clients;

public interface IOrdersClient
{
    Task<Resposta<PedidoResponse>> AtualizarPedidoPagamentoAsync(AtualizarPedidoPagamentoRequest request, CancellationToken cancellationToken);
}
