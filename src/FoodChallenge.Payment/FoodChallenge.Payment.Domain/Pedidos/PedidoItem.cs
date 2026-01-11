using FoodChallenge.Payment.Domain.Entities;

namespace FoodChallenge.Payment.Domain.Pedidos;

public class PedidoItem : DomainBase
{
    public Guid? IdProduto { get; set; }
    public Guid? IdPedido { get; set; }
    public string Codigo { get; set; }
    public decimal Valor { get; set; }
    public int Quantidade { get; set; }
}
