namespace FoodChallenge.Payment.Application.Pagamentos.Interfaces;

public interface IObtemImagemQrCodePagamentoUseCase
{
    Task<byte[]> ExecutarAsync(Guid idPedido, CancellationToken cancellationToken);
}
