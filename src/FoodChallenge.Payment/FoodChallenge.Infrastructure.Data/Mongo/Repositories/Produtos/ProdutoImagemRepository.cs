using FoodChallenge.Infrastructure.Data.Mongo.Context;
using FoodChallenge.Infrastructure.Data.Mongo.Repositories.Produtos.Interfaces;
using MongoDB.Driver;

namespace FoodChallenge.Infrastructure.Data.Mongo.Repositories.Produtos;

public class ProdutoImagemRepository : RepositoryBase<ProdutoImagemDocument>, IProdutoImagemRepository
{
    public ProdutoImagemRepository(MongoDbContext context)
        : base(context, "produtoImagens")
    {
    }

    public async Task<ICollection<ProdutoImagemDocument>> GetByProductIdAsync(Guid idProduct, CancellationToken cancellationToken, bool tracking = false)
    {
        return await _collection.Find(e => e.IdProduto == idProduct).ToListAsync(cancellationToken);
    }
}

