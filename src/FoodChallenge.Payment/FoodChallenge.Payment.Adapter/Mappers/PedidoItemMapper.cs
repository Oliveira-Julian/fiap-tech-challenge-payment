using FoodChallenge.Infrastructure.Clients.Orders.Models;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Adapter.Mappers;

public static class PedidoItemMapper
{
    public static PedidoItem ToDomain(PedidoItemResponse itemResponse)
    {
        if (itemResponse is null) return default;

        return new PedidoItem()
        {
            Id = itemResponse.Id,
            IdProduto = itemResponse.IdProduto,
            Codigo = itemResponse.Codigo,
            Quantidade = itemResponse.Quantidade,
            Valor = itemResponse.Valor
        };
    }
}
