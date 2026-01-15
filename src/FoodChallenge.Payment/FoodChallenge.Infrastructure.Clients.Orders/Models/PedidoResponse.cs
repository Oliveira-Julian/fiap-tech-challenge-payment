using FoodChallenge.Infrastructure.Clients.Orders.Enums;

namespace FoodChallenge.Infrastructure.Clients.Orders.Models;

public class PedidoResponse
{
    public Guid? Id { get; set; }
    public string Codigo { get; set; }
    public decimal ValorTotal { get; set; }
    public PedidoStatus? Status { get; set; }
    public string DescricaoStatus { get; set; }
    public IEnumerable<PedidoItemResponse> Itens { get; set; }
}
