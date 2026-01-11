using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Context;
using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Pedidos.Interfaces;
using MongoDB.Driver;

namespace FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Pedidos;

public class PedidoPagamentoRepository : RepositoryBase<PagamentoEntity>, IPedidoPagamentoRepository
{
    public PedidoPagamentoRepository(MongoDbContext context)
        : base(context, "pagamentos")
    {
    }

    public async Task<PagamentoEntity> GetByIdMercadoPagoPagamentoAsync(string idMercadoPagoPagamento, CancellationToken cancellationToken, bool tracking = false)
    {
        return await _collection.Find(e => e.IdMercadoPagoPagamento == idMercadoPagoPagamento).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagamentoEntity> ObterPagamentoPorIdPedidoAsync(Guid idPedido, CancellationToken cancellationToken, bool tracking = false)
    {
        return await _collection.Find(e => e.IdPedido == idPedido).FirstOrDefaultAsync(cancellationToken);
    }
}
