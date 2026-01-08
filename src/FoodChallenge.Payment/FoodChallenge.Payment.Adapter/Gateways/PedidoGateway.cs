using FoodChallenge.Infrastructure.Data.Postgres.EntityFramework.Repositories.Pedidos.Interfaces;
using FoodChallenge.Payment.Adapter.Mappers;
using FoodChallenge.Payment.Application.Pedidos;
using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Adapter.Gateways;

public class PedidoGateway(IPedidoRepository pedidoDataSource) : IPedidoGateway
{
    public async Task<Pedido> ObterPedidoComRelacionamentosAsync(Guid idPedido, CancellationToken cancellationToken, bool tracking = false)
    {
        var pedidoEntity = await pedidoDataSource.GetByIdWithRelationsAsync(idPedido, cancellationToken, tracking);
        return PedidoMapper.ToDomain(pedidoEntity);
    }

    public async Task<Pedido> ObterPedidoAsync(Guid idPedido, CancellationToken cancellationToken, bool tracking = false)
    {
        var pedidoEntity = await pedidoDataSource.GetByIdAsync(idPedido, cancellationToken, tracking);
        return PedidoMapper.ToDomain(pedidoEntity);
    }

    public void AtualizarPedido(Pedido pedido)
    {
        var pedidoEntity = PedidoMapper.ToEntity(pedido);
        pedidoDataSource.Update(pedidoEntity);
    }

    public async Task<IEnumerable<Pedido>> ObterPedidosPorProdutoAsync(Guid idProduto, IEnumerable<PedidoStatus> pedidosStatus, CancellationToken cancellationToken, bool tracking = false)
    {
        var pedidosEntity = await pedidoDataSource.ObterPedidosPorProdutoAsync(idProduto, pedidosStatus?.Select(status => (int)status), cancellationToken, tracking);
        return pedidosEntity?.Select(PedidoMapper.ToDomain);
    }
}
