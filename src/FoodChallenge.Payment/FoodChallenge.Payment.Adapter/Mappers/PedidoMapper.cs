using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Pedidos;
using FoodChallenge.Payment.Adapter.Presenters;
using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Adapter.Mappers;

public static class PedidoMapper
{
    public static Pedido ToDomain(PedidoEntity pedidoEntity)
    {
        if (pedidoEntity is null) return default;

        return new Pedido()
        {
            Id = pedidoEntity.Id,
            IdPagamento = pedidoEntity.IdPagamento,
            Codigo = pedidoEntity.Codigo,
            DataAtualizacao = pedidoEntity.DataAtualizacao,
            DataCriacao = pedidoEntity.DataCriacao,
            DataExclusao = pedidoEntity.DataExclusao,
            Ativo = pedidoEntity.Ativo,
            Status = (PedidoStatus)pedidoEntity.Status,
            ValorTotal = pedidoEntity.ValorTotal,
            Itens = pedidoEntity.Itens?.Select(PedidoItemMapper.ToDomain)
        };
    }

    public static PedidoEntity ToEntity(Pedido pedido)
    {
        if (pedido is null) return default;

        return new PedidoEntity()
        {
            Id = pedido.Id,
            IdPagamento = pedido.IdPagamento,
            Codigo = pedido.Codigo,
            DataAtualizacao = pedido.DataAtualizacao,
            DataCriacao = pedido.DataCriacao,
            DataExclusao = pedido.DataExclusao,
            Ativo = pedido.Ativo,
            Status = (int)pedido.Status,
            ValorTotal = pedido.ValorTotal,
            Itens = pedido.Itens?.Select(PedidoItemMapper.ToEntity)?.ToList()
        };
    }
}
