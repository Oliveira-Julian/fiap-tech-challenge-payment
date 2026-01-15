using FoodChallenge.Infrastructure.Clients.Orders.Clients;
using FoodChallenge.Payment.Adapter.Mappers;
using FoodChallenge.Payment.Application.Pedidos;
using FoodChallenge.Payment.Domain.Globalization;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Adapter.Gateways;

public class PedidoGateway(IOrdersClient ordersClient) : IPedidoGateway
{
    public async Task<Pedido> ObterPedidoAsync(Guid idPedido, CancellationToken cancellationToken)
    {
        var response = await ordersClient.ObterPedidoAsync(idPedido, cancellationToken);

        if (response is null || !response.Sucesso)
            throw new Exception(Textos.ErroInesperado);

        return PedidoMapper.ToDomain(response.Dados);
    }

    public async Task ConfirmarPagamentoAsync(Guid idPedido, CancellationToken cancellationToken)
    {
        var response = await ordersClient.ConfirmarPagamentoAsync(idPedido, cancellationToken);

        if (response is null || !response.Sucesso)
            throw new Exception(Textos.ErroInesperado);
    }
}
