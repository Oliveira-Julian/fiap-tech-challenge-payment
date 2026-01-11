using FoodChallenge.Infrastructure.Data.Mongo.Bases;

namespace FoodChallenge.Infrastructure.Data.Mongo.Repositories.Clientes.Interfaces;

public interface IClienteRepository : IRepositoryBase<ClienteEntity>
{
    Task<ClienteEntity> ObterPorCpfAsync(
        string cpf,
        CancellationToken cancellationToken,
        bool tracking = false);
}

