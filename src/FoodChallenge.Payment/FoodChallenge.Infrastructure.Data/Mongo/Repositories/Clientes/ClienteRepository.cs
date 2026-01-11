using FoodChallenge.Infrastructure.Data.Mongo.Context;
using FoodChallenge.Infrastructure.Data.Mongo.Repositories.Clientes.Interfaces;
using MongoDB.Driver;

namespace FoodChallenge.Infrastructure.Data.Mongo.Repositories.Clientes;

public class ClienteRepository : RepositoryBase<ClienteEntity>, IClienteRepository
{
    public ClienteRepository(MongoDbContext context)
        : base(context, "clientes")
    {
    }

    public async Task<ClienteEntity> ObterPorCpfAsync(
        string cpf,
        CancellationToken cancellationToken,
        bool tracking = false)
    {
        return await _collection.Find(c => c.Cpf == cpf).FirstOrDefaultAsync(cancellationToken);
    }
}

