using FoodChallenge.Infrastructure.Data.Postgres.EntityFramework.Repositories.Preparos.Interfaces;
using FoodChallenge.Payment.Adapter.Mappers;
using FoodChallenge.Payment.Application.Preparos;
using FoodChallenge.Payment.Domain.Preparos;

namespace FoodChallenge.Payment.Adapter.Gateways;

public class OrdemPedidoGateway(
    IOrdemPedidoRepository ordemPedidoDataSource) : IOrdemPedidoGateway
{
    public async Task<OrdemPedido> CadastrarOrdemPedidoAsync(OrdemPedido ordemPedido, CancellationToken cancellationToken)
    {
        var ordemPedidoEntity = OrdemPedidoMapper.ToEntity(ordemPedido);
        ordemPedidoEntity = await ordemPedidoDataSource.AddAsync(ordemPedidoEntity, cancellationToken);
        return OrdemPedidoMapper.ToDomain(ordemPedidoEntity);
    }

    public void AtualizarOrdemPedido(OrdemPedido ordemPedido)
    {
        var ordemPedidoEntity = OrdemPedidoMapper.ToEntity(ordemPedido);
        ordemPedidoDataSource.Update(ordemPedidoEntity);
    }

    public async Task<OrdemPedido> ObterOrdemPedidoPorIdAsync(Guid idOrdemPedido, CancellationToken cancellationToken, bool tracking = false)
    {
        var ordemPedidoEntity = await ordemPedidoDataSource.GetByIdAsync(idOrdemPedido, cancellationToken, tracking);
        return OrdemPedidoMapper.ToDomain(ordemPedidoEntity);
    }

    public async Task<OrdemPedido> ObterPedidoComRelacionamentosAsync(Guid idOrdemPedido, CancellationToken cancellationToken, bool tracking = false)
    {
        var ordemPedidoEntity = await ordemPedidoDataSource.GetByIdWithRelationsAsync(idOrdemPedido, cancellationToken, tracking);
        return OrdemPedidoMapper.ToDomain(ordemPedidoEntity);
    }
}
