using FoodChallenge.Common.Extensions;
using FoodChallenge.Payment.Application.Pedidos.Models.Responses;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Adapter.Presenters;

public static class PedidoPresenter
{
    public static PedidoResponse ToResponse(Pedido pedido)
    {
        if (pedido is null) return default;

        return new PedidoResponse()
        {
            Id = pedido.Id,
            Codigo = pedido.Codigo,
            Itens = pedido.Itens?.Select(PedidoItemPresenter.ToResponse),
            ValorTotal = pedido.ValorTotal,
            Status = pedido.Status,
            DescricaoStatus = pedido.Status.GetDescription(),
            Pagamento = PagamentoPresenter.ToResponse(pedido.Pagamento)
        };
    }
}
