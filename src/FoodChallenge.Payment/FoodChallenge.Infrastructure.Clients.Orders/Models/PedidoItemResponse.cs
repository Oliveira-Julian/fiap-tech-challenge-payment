namespace FoodChallenge.Infrastructure.Clients.Orders.Models;

public sealed class PedidoItemResponse
{
    public Guid? Id { get; set; }
    public Guid? IdProduto { get; set; }
    public string Codigo { get; set; }
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
}
