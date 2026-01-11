using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Bases;

namespace FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Pedidos.Interfaces;

public interface IPedidoRepository : IRepositoryBase<PedidoEntity>
{
    Task<PedidoEntity> GetByIdWithRelationsAsync(Guid id, CancellationToken cancellationToken, bool tracking = false);
    Task<IEnumerable<PedidoEntity>> ObterPedidosPorProdutoAsync(Guid idProduto,
        IEnumerable<int> pedidosStatus,
        CancellationToken cancellationToken,
        bool tracking = false);
}
