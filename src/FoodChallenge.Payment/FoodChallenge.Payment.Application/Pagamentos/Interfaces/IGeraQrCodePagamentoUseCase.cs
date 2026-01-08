using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Application.Pagamentos.Interfaces;

public interface IGeraQrCodePagamentoUseCase
{
    Task<Pedido> ExecutarAsync(Guid idPedido, CancellationToken cancellationToken);
}
