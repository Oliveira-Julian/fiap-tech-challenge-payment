using FoodChallenge.CrossCutting.Paging;

namespace FoodChallenge.Infrastructure.Data.Mongo.Repositories.Preparos;

public class OrdemPedidoEntityFilter
{
    [FilterBy(nameof(OrdemPedidoEntity.Status))]
    public IEnumerable<int> Status { get; set; }

    [IgnoreFilter]
    public int? StatusPedidoDiferente { get; set; }
}

