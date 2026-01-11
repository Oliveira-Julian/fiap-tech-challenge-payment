using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Context;
using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Pedidos.Interfaces;
using MongoDB.Driver;

namespace FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Pedidos;

public class PedidoRepository : RepositoryBase<PedidoEntity>, IPedidoRepository
{
    public PedidoRepository(MongoDbContext context)
        : base(context, "pedidos")
    {
    }

    public async Task<PedidoEntity> GetByIdWithRelationsAsync(Guid id, CancellationToken cancellationToken, bool tracking = false)
    {
        // No MongoDB, os itens já estão embarcados no documento do pedido
        return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<PedidoEntity>> ObterPedidosPorProdutoAsync(Guid idProduto,
        IEnumerable<int> pedidosStatus,
        CancellationToken cancellationToken,
        bool tracking = false)
    {
        var statusList = pedidosStatus.ToList();
        var filter = Builders<PedidoEntity>.Filter.And(
            Builders<PedidoEntity>.Filter.In(p => p.Status, statusList),
            Builders<PedidoEntity>.Filter.ElemMatch(p => p.Itens, i => i.IdProduto == idProduto)
        );

        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }
}
