namespace FoodChallenge.Infrastructure.Clients.Orders.Models;

public sealed class PedidoResponse
{
    public Guid? Id { get; set; }
    public int Status { get; set; }
    public string DescricaoStatus { get; set; }
}
