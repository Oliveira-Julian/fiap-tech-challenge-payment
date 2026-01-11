using FoodChallenge.CrossCutting.Paging;

namespace FoodChallenge.Infrastructure.Data.Mongo.Repositories.Clientes;

public class ClienteEntityFilter
{
    [FilterBy(nameof(ClienteEntity.Cpf), FilterType.Equals)]
    public string Cpf { get; set; }

    [FilterBy(nameof(ClienteEntity.Ativo), FilterType.Equals)]
    public bool Ativo { get; set; } = true;
}

