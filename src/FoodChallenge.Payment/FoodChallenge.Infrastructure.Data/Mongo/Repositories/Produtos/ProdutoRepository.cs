using FoodChallenge.Infrastructure.Data.Mongo.Context;
using FoodChallenge.Infrastructure.Data.Mongo.Repositories.Produtos.Interfaces;

namespace FoodChallenge.Infrastructure.Data.Mongo.Repositories.Produtos;

public class ProdutoRepository : RepositoryBase<ProdutoEntity>, IProdutoRepository
{
    public ProdutoRepository(MongoDbContext context)
        : base(context, "produtos")
    {
    }
}

