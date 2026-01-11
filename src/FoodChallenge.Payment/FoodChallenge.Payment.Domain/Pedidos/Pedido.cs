using FoodChallenge.Payment.Domain.Entities;
using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Pagamentos;

namespace FoodChallenge.Payment.Domain.Pedidos;

public class Pedido : DomainBase
{
    public Guid? IdPagamento { get; set; }
    public string Codigo { get; set; }
    public Pagamento Pagamento { get; set; }
    public IEnumerable<PedidoItem> Itens { get; set; }
    public decimal ValorTotal { get; set; }
    public PedidoStatus Status { get; set; }
    
    public bool PodeSerPago() => Status == PedidoStatus.Recebido;

    public void AtualizarStatusPago()
    {
        Status = PedidoStatus.NaFila;
        Atualizar();
    }
}
