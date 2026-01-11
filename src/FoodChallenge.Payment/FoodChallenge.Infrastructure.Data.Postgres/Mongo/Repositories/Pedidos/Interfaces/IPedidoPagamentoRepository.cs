using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Bases;

namespace FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Pedidos.Interfaces;

public interface IPedidoPagamentoRepository : IRepositoryBase<PagamentoEntity>
{
    Task<PagamentoEntity> GetByIdMercadoPagoPagamentoAsync(string idMercadoPagoPagamento, CancellationToken cancellationToken, bool tracking = false);
    Task<PagamentoEntity> ObterPagamentoPorIdPedidoAsync(Guid idPedido, CancellationToken cancellationToken, bool tracking = false);
}
