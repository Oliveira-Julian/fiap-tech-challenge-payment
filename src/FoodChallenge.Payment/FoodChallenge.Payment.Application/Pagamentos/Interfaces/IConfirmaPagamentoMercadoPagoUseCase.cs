using FoodChallenge.Payment.Domain.Pagamentos;

namespace FoodChallenge.Payment.Application.Pagamentos.Interfaces;

public interface IConfirmaPagamentoMercadoPagoUseCase
{
    Task<Pagamento> ExecutarAsync(NotificacaoMercadoPago notificacaoMercadoPago, CancellationToken cancellationToken);
}
