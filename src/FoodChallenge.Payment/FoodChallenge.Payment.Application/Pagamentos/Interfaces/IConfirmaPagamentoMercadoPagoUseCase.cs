using FoodChallenge.Payment.Domain.Pagamentos;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Application.Pagamentos.Interfaces;

public interface IConfirmaPagamentoMercadoPagoUseCase
{
    Task<Pedido> ExecutarAsync(NotificacaoMercadoPago notificacaoMercadoPago, CancellationToken cancellationToken);
}
