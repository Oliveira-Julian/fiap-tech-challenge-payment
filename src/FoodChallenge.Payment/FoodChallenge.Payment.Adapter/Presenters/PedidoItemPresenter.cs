using FoodChallenge.Payment.Application.Pedidos.Models.Responses;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Adapter.Presenters;

public static class PedidoItemPresenter
{
    public static PedidoItemResponse ToResponse(PedidoItem item)
    {
        if (item is null) return default;

        return new PedidoItemResponse()
        {
            Id = item.Id,
            IdProduto = item.IdProduto,
            Codigo = item.Codigo,
            Quantidade = item.Quantidade,
            Valor = item.Valor
        };
    }
}
