using FoodChallenge.Infrastructure.Clients.Orders.Clients;
using FoodChallenge.Infrastructure.Clients.Orders.Models;
using FoodChallenge.Payment.Application.Pedidos;

namespace FoodChallenge.Payment.Adapter.Gateways;

public class PedidoGateway(IOrdersClient ordersClient) : IPedidoGateway
{
    public void AtualizarPedido(Guid idPedido, string status)
    {
        ordersClient.AtualizarPedidoPagamentoAsync(new AtualizarPedidoPagamentoRequest(idPedido, status), CancellationToken.None);
    }
}
