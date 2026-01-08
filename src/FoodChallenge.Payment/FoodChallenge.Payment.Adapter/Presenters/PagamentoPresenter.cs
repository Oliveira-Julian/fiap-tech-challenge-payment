using FoodChallenge.Common.Extensions;
using FoodChallenge.Payment.Application.Pagamentos.Models.Responses;
using FoodChallenge.Payment.Domain.Pagamentos;

namespace FoodChallenge.Payment.Adapter.Presenters;

public static class PagamentoPresenter
{
    public static PagamentoResponse ToResponse(Pagamento pagamento)
    {
        if (pagamento is null) return default;

        return new PagamentoResponse
        {
            QrCode = pagamento.QrCode,
            Status = (int)pagamento.Status,
            DescricaoStatus = pagamento.Status.GetDescription(),
            IdMercadoPagoPagamento = pagamento.IdMercadoPagoPagamento
        };
    }
}
