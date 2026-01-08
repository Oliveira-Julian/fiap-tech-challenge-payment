using FoodChallenge.CrossCutting.Paging;
using FoodChallenge.Infrastructure.Data.Postgres.EntityFramework.Repositories.Preparos;
using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Preparos;

namespace FoodChallenge.Payment.Adapter.Mappers;

public static class OrdemPedidoMapper
{
    public static OrdemPedido ToDomain(OrdemPedidoEntity ordemPedidoEntity)
    {
        if (ordemPedidoEntity is null) return default;

        return new OrdemPedido()
        {
            Id = ordemPedidoEntity.Id,
            IdPedido = ordemPedidoEntity.IdPedido,
            DataAtualizacao = ordemPedidoEntity.DataAtualizacao,
            DataCriacao = ordemPedidoEntity.DataCriacao,
            DataExclusao = ordemPedidoEntity.DataExclusao,
            Ativo = ordemPedidoEntity.Ativo,
            Status = (PreparoStatus)ordemPedidoEntity.Status,
            DataInicioPreparacao = ordemPedidoEntity.DataInicioPreparacao,
            DataFimPreparacao = ordemPedidoEntity.DataFimPreparacao,
            Pedido = PedidoMapper.ToDomain(ordemPedidoEntity.Pedido)
        };
    }

    public static OrdemPedidoEntity ToEntity(OrdemPedido ordemPedido)
    {
        if (ordemPedido is null) return default;

        return new OrdemPedidoEntity
        {
            Id = ordemPedido.Id,
            IdPedido = ordemPedido.IdPedido,
            DataAtualizacao = ordemPedido.DataAtualizacao,
            DataCriacao = ordemPedido.DataCriacao,
            DataExclusao = ordemPedido.DataExclusao,
            Ativo = ordemPedido.Ativo,
            Status = (int)ordemPedido.Status,
            DataInicioPreparacao = ordemPedido.DataInicioPreparacao,
            DataFimPreparacao = ordemPedido.DataFimPreparacao,
            Pedido = PedidoMapper.ToEntity(ordemPedido.Pedido)
        };
    }

    public static Pagination<OrdemPedido> ToPagination(Pagination<OrdemPedidoEntity> pagination)
    {
        if (pagination is null) return default;

        return new Pagination<OrdemPedido>(
            pagination.Page,
            pagination.SizePage,
            pagination.TotalRecords,
            pagination.Records?.Select(ToDomain));
    }
}
