using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Context;
using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Produtos.Interfaces;

namespace FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Produtos;

public class ProdutoRepository : RepositoryBase<ProdutoEntity>, IProdutoRepository
{
    public ProdutoRepository(MongoDbContext context)
        : base(context, "produtos")
    {
    }
}
