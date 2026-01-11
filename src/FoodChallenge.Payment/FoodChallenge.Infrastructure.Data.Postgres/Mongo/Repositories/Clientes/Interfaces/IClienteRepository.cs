using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Bases;

namespace FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Clientes.Interfaces;

public interface IClienteRepository : IRepositoryBase<ClienteEntity>
{
    Task<ClienteEntity> ObterPorCpfAsync(
        string cpf,
        CancellationToken cancellationToken,
        bool tracking = false);
}
