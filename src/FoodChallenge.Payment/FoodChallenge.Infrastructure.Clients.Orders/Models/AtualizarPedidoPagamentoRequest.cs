namespace FoodChallenge.Infrastructure.Clients.Orders.Models;

public sealed class AtualizarPedidoPagamentoRequest
{
    public Guid? IdPedido { get; set; }
    public string status { get; set; }
}
