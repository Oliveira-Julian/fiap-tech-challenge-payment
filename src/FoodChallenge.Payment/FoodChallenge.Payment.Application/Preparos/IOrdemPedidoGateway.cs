using FoodChallenge.Payment.Domain.Preparos;

namespace FoodChallenge.Payment.Application.Preparos;

public interface IOrdemPedidoGateway
{
    Task<OrdemPedido> CadastrarOrdemPedidoAsync(OrdemPedido ordemPedido, CancellationToken cancellationToken);
    void AtualizarOrdemPedido(OrdemPedido ordemPedido);
    Task<OrdemPedido> ObterOrdemPedidoPorIdAsync(Guid idOrdemPedido, CancellationToken cancellationToken, bool tracking = false);
    Task<OrdemPedido> ObterPedidoComRelacionamentosAsync(Guid idOrdemPedido, CancellationToken cancellationToken, bool tracking = false);
}
