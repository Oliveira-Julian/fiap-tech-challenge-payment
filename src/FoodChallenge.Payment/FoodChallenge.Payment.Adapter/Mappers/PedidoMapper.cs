using FoodChallenge.Infrastructure.Clients.Orders.Models;
using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Adapter.Mappers;

public static class PedidoMapper
{
    public static Pedido ToDomain(PedidoResponse pedidoResponse)
    {
        if (pedidoResponse is null) return default;

        return new Pedido()
        {
            Id = pedidoResponse.Id,
            Codigo = pedidoResponse.Codigo,
            Status = (PedidoStatus)pedidoResponse.Status,
            ValorTotal = pedidoResponse.ValorTotal,
            Itens = pedidoResponse.Itens?.Select(PedidoItemMapper.ToDomain)
        };
    }
}
