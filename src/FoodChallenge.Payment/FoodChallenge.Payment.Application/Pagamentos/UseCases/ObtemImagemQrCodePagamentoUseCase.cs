using FoodChallenge.Common.Validators;
using FoodChallenge.Payment.Application.Helpers;
using FoodChallenge.Payment.Application.Pagamentos.Interfaces;
using FoodChallenge.Payment.Domain.Constants;
using FoodChallenge.Payment.Domain.Globalization;
using FoodChallenge.Payment.Domain.Pagamentos;
using Serilog;

namespace FoodChallenge.Payment.Application.Pagamentos.UseCases;

public class ObtemImagemQrCodePagamentoUseCase(
    ValidationContext validationContext,
    IPagamentoGateway pagamentoGateway) : IObtemImagemQrCodePagamentoUseCase
{
    private readonly ILogger logger = Log.ForContext<ObtemImagemQrCodePagamentoUseCase>();

    public async Task<byte[]> ExecutarAsync(Guid idPedido, CancellationToken cancellationToken)
    {
        logger.Information(Logs.InicioExecucaoServico, nameof(ObtemImagemQrCodePagamentoUseCase), nameof(ExecutarAsync));

        var pagamento = await pagamentoGateway.ObterPagamentoPorIdPedidoAsync(idPedido, cancellationToken);
        if (pagamento is null)
        {
            validationContext.AddValidation(string.Format(Textos.NaoEncontrado, nameof(Pagamento)));
            return default;
        }

        if (string.IsNullOrWhiteSpace(pagamento.QrCode))
        {
            validationContext.AddValidation(string.Format(Textos.QrCodeNaoFoiGerado, idPedido));
            return default;
        }

        logger.Information(Logs.FimExecucaoServico, nameof(ObtemImagemQrCodePagamentoUseCase), nameof(ExecutarAsync), pagamento);

        return QrCodeHelper.GerarImagem(pagamento.QrCode);
    }
}
