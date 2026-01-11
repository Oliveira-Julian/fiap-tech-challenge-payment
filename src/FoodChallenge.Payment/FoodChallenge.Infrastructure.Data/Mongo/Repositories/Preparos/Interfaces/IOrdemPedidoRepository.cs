using FoodChallenge.CrossCutting.Paging;
using FoodChallenge.Infrastructure.Data.Mongo.Bases;

namespace FoodChallenge.Infrastructure.Data.Mongo.Repositories.Preparos.Interfaces;

public interface IOrdemPedidoRepository : IRepositoryBase<OrdemPedidoEntity>
{
    Task<OrdemPedidoEntity> GetByIdWithRelationsAsync(Guid id, CancellationToken cancellationToken, bool tracking = false);

    Task<Pagination<OrdemPedidoEntity>> GetOrdersOrderDefaultAsync(Filter<OrdemPedidoEntityFilter> filter, CancellationToken cancellationToken);
}

