using FoodChallenge.Payment.Domain.Enums;

namespace FoodChallenge.Payment.Application.Preparos.Models.Responses;

public class OrdemResponse
{
    public Guid? Id { get; set; }
    public OrdemPedidoResponse Pedido { get; set; }
    public PreparoStatus Status { get; set; }
    public string DescricaoStatus { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataInicioPreparacao { get; set; }
    public DateTime? DataFimPreparacao { get; set; }
}
