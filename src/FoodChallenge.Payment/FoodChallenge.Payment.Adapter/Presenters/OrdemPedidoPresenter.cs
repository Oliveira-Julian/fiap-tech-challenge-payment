using FoodChallenge.Common.Extensions;
using FoodChallenge.Payment.Application.Preparos.Models.Responses;
using FoodChallenge.Payment.Domain.Pedidos;
using FoodChallenge.Payment.Domain.Preparos;

namespace FoodChallenge.Payment.Adapter.Presenters;

public class OrdemPedidoPresenter
{
    public static OrdemResponse ToResponse(OrdemPedido ordemPedido)
    {
        if (ordemPedido is null) return default;

        return new OrdemResponse()
        {
            Id = ordemPedido.Id,
            Pedido = ToResponse(ordemPedido.Pedido),
            Status = ordemPedido.Status,
            DescricaoStatus = ordemPedido.Status.GetDescription(),
            DataCriacao = ordemPedido.DataCriacao,
            DataInicioPreparacao = ordemPedido.DataInicioPreparacao,
            DataFimPreparacao = ordemPedido.DataFimPreparacao
        };
    }

    public static OrdemPedidoResponse ToResponse(Pedido pedido)
    {
        if (pedido is null) return default;

        return new OrdemPedidoResponse()
        {
            Id = pedido.Id,
            Itens = pedido.Itens?.Select(PedidoItemPresenter.ToResponse),
            ValorTotal = pedido.ValorTotal,
            Status = pedido.Status,
            DescricaoStatus = pedido.Status.GetDescription()
        };
    }
}
