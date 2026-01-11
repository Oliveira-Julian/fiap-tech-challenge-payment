using FoodChallenge.CrossCutting.Paging;

namespace FoodChallenge.Infrastructure.Data.Mongo.Repositories.Pedidos;

public class PedidoEntityFilter
{
    [FilterBy(nameof(PedidoEntity.Id), FilterType.Equals)]
    public Guid? Id { get; set; }

    [FilterBy(nameof(PedidoEntity.IdCliente), FilterType.Equals)]
    public Guid? IdCliente { get; set; }

    [FilterBy(nameof(PedidoEntity.Ativo), FilterType.Equals)]
    public bool Ativo { get; set; } = true;
}

