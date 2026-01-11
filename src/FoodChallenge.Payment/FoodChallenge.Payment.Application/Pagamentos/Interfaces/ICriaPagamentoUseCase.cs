using FoodChallenge.Payment.Application.Pagamentos.Models.Requests;
using FoodChallenge.Payment.Domain.Pagamentos;

namespace FoodChallenge.Payment.Application.Pagamentos.Interfaces;

public interface ICriaPagamentoUseCase
{
    Task<Pagamento> ExecutarAsync(CriarPagamentoRequest request, CancellationToken cancellationToken);
}
