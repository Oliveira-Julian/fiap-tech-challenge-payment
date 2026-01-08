using FoodChallenge.Common.Validators;
using FoodChallenge.Payment.Application.Helpers;
using FoodChallenge.Payment.Application.Pagamentos.Interfaces;
using FoodChallenge.Payment.Application.Pedidos;
using FoodChallenge.Payment.Domain.Constants;
using FoodChallenge.Payment.Domain.Globalization;
using FoodChallenge.Payment.Domain.Pedidos;
using Serilog;

namespace FoodChallenge.Payment.Application.Pagamentos.UseCases;

public class ObtemImagemQrCodePagamentoUseCase(
    ValidationContext validationContext,
    IPedidoGateway pedidoGateway) : IObtemImagemQrCodePagamentoUseCase
{
    private readonly ILogger logger = Log.ForContext<ObtemImagemQrCodePagamentoUseCase>();

    public async Task<byte[]> ExecutarAsync(Guid idPedido, CancellationToken cancellationToken)
    {
        logger.Information(Logs.InicioExecucaoServico, nameof(ObtemImagemQrCodePagamentoUseCase), nameof(ExecutarAsync));

        var pedido = await pedidoGateway.ObterPedidoComRelacionamentosAsync(idPedido, cancellationToken);
        if (pedido is null)
        {
            validationContext.AddValidation(string.Format(Textos.NaoEncontrado, nameof(Pedido)));
            return default;
        }

        if (pedido.Pagamento is null)
        {
            validationContext.AddValidation(string.Format(Textos.QrCodeNaoFoiGerado, pedido.Id));
            return default;
        }

        logger.Information(Logs.FimExecucaoServico, nameof(ObtemImagemQrCodePagamentoUseCase), nameof(ExecutarAsync), pedido.Pagamento);

        return QrCodeHelper.GerarImagem(pedido.Pagamento.QrCode);
    }
}
