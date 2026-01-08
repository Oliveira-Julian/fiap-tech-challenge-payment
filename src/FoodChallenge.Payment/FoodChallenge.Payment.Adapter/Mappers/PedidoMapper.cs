using FoodChallenge.Infrastructure.Data.Postgres.EntityFramework.Repositories.Pedidos;
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
            IdCliente = pedidoEntity.IdCliente,
            Codigo = pedidoEntity.Codigo,
            DataAtualizacao = pedidoEntity.DataAtualizacao,
            DataCriacao = pedidoEntity.DataCriacao,
            DataExclusao = pedidoEntity.DataExclusao,
            Ativo = pedidoEntity.Ativo,
            Status = (PedidoStatus)pedidoEntity.Status,
            ValorTotal = pedidoEntity.ValorTotal,
            Cliente = ClienteMapper.ToDomain(pedidoEntity.Cliente),
            Pagamento = PagamentoMapper.ToDomain(pedidoEntity.Pagamento),
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
            IdCliente = pedido.IdCliente,
            Codigo = pedido.Codigo,
            DataAtualizacao = pedido.DataAtualizacao,
            DataCriacao = pedido.DataCriacao,
            DataExclusao = pedido.DataExclusao,
            Ativo = pedido.Ativo,
            Status = (int)pedido.Status,
            ValorTotal = pedido.ValorTotal,
            Pagamento = PagamentoMapper.ToEntity(pedido.Pagamento),
            Itens = pedido.Itens?.Select(PedidoItemMapper.ToEntity)?.ToList()
        };
    }

    public static Pedido ToDomain(Guid? idCliente)
    {
        if (!idCliente.HasValue) return default;

        return new Pedido()
        {
            IdCliente = idCliente.Value
        };
    }
}
