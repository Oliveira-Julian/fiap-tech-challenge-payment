using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Pedidos;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Adapter.Mappers;

public static class PedidoItemMapper
{
    public static PedidoItem ToDomain(PedidoItemEntity pedidoItemEntity)
    {
        if (pedidoItemEntity is null)
            return null;

        return new PedidoItem()
        {
            Id = pedidoItemEntity.Id,
            IdPedido = pedidoItemEntity.IdPedido,
            IdProduto = pedidoItemEntity.IdProduto,
            Codigo = pedidoItemEntity.Codigo,
            DataAtualizacao = pedidoItemEntity.DataAtualizacao,
            DataCriacao = pedidoItemEntity.DataCriacao,
            DataExclusao = pedidoItemEntity.DataExclusao,
            Ativo = pedidoItemEntity.Ativo,
            Quantidade = pedidoItemEntity.Quantidade,
            Valor = pedidoItemEntity.Valor
        };
    }

    public static PedidoItemEntity ToEntity(PedidoItem pedidoItem)
    {
        if (pedidoItem is null)
            return null;

        return new PedidoItemEntity()
        {
            Id = pedidoItem.Id,
            IdPedido = pedidoItem.IdPedido,
            IdProduto = pedidoItem.IdProduto,
            Codigo = pedidoItem.Codigo,
            DataAtualizacao = pedidoItem.DataAtualizacao,
            DataCriacao = pedidoItem.DataCriacao,
            DataExclusao = pedidoItem.DataExclusao,
            Ativo = pedidoItem.Ativo,
            Quantidade = pedidoItem.Quantidade,
            Valor = pedidoItem.Valor
        };
    }
}
