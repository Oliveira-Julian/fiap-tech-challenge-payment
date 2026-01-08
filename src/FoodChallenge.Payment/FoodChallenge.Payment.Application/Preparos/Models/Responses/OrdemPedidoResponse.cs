using FoodChallenge.Payment.Application.Pedidos.Models.Responses;
using FoodChallenge.Payment.Domain.Enums;

namespace FoodChallenge.Payment.Application.Preparos.Models.Responses;

public class OrdemPedidoResponse
{
    public Guid? Id { get; set; }
    public decimal? ValorTotal { get; set; }
    public PedidoStatus? Status { get; set; }
    public string DescricaoStatus { get; set; }
    public IEnumerable<PedidoItemResponse> Itens { get; set; }
}
