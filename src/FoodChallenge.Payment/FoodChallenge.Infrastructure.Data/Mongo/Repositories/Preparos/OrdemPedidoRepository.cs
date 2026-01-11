using FoodChallenge.CrossCutting.Paging;
using FoodChallenge.Infrastructure.Data.Mongo.Context;
using FoodChallenge.Infrastructure.Data.Mongo.Repositories.Pedidos;
using FoodChallenge.Infrastructure.Data.Mongo.Repositories.Preparos.Interfaces;
using MongoDB.Driver;

namespace FoodChallenge.Infrastructure.Data.Mongo.Repositories.Preparos;

public class OrdemPedidoRepository : RepositoryBase<OrdemPedidoEntity>, IOrdemPedidoRepository
{
    private readonly IMongoCollection<PedidoEntity> _pedidoCollection;

    public OrdemPedidoRepository(MongoDbContext context)
        : base(context, "ordensPedido")
    {
        _pedidoCollection = context.GetCollection<PedidoEntity>("pedidos");
    }

    public async Task<OrdemPedidoEntity> GetByIdWithRelationsAsync(Guid id, CancellationToken cancellationToken, bool tracking = false)
    {
        // No MongoDB, relacionamentos são tratados no nível da aplicação ou com lookup
        return await _collection.Find(o => o.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Pagination<OrdemPedidoEntity>> GetOrdersOrderDefaultAsync(Filter<OrdemPedidoEntityFilter> filter, CancellationToken cancellationToken)
    {
        var builder = Builders<OrdemPedidoEntity>.Filter;
        var filterDefinition = BuildFilterDefinition(filter.Fields);

        // Se houver filtro de StatusPedidoDiferente, precisamos fazer lookup
        if (filter.Fields?.StatusPedidoDiferente > 0)
        {
            // Buscar IDs de pedidos que NÃO têm o status especificado
            var pedidosFilter = Builders<PedidoEntity>.Filter.Ne(p => p.Status, filter.Fields.StatusPedidoDiferente.Value);
            var pedidosValidos = await _pedidoCollection.Find(pedidosFilter).Project(p => p.Id).ToListAsync(cancellationToken);
            
            filterDefinition = builder.And(filterDefinition, builder.In(o => o.IdPedido, pedidosValidos));
        }

        var sortDefinition = Builders<OrdemPedidoEntity>.Sort
            .Descending(x => x.Status)
            .Ascending(x => x.DataInicioPreparacao)
            .Ascending(x => x.DataCriacao);

        var total = await _collection.CountDocumentsAsync(filterDefinition, cancellationToken: cancellationToken);

        var items = await _collection
            .Find(filterDefinition)
            .Sort(sortDefinition)
            .Skip((filter.Page - 1) * filter.SizePage)
            .Limit(filter.SizePage)
            .ToListAsync(cancellationToken);

        return new Pagination<OrdemPedidoEntity>(filter.Page, items.Count, (int)total, items);
    }
}

