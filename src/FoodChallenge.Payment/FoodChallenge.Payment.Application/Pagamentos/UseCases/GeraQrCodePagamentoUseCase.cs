using FoodChallenge.Common.Interfaces;
using FoodChallenge.Common.Validators;
using FoodChallenge.Payment.Application.Pagamentos.Interfaces;
using FoodChallenge.Payment.Application.Pedidos;
using FoodChallenge.Payment.Domain.Constants;
using FoodChallenge.Payment.Domain.Globalization;
using FoodChallenge.Payment.Domain.Pedidos;
using Serilog;

namespace FoodChallenge.Payment.Application.Pagamentos.UseCases;

public sealed class GeraQrCodePagamentoUseCase(
    ValidationContext validationContext,
    IUnitOfWork unitOfWork,
    IPedidoGateway pedidoGateway,
    IPagamentoGateway pagamentoGateway) : IGeraQrCodePagamentoUseCase
{
    private readonly ILogger logger = Log.ForContext<GeraQrCodePagamentoUseCase>();

    public async Task<Pedido> ExecutarAsync(Guid idPedido, CancellationToken cancellationToken)
    {
        try
        {
            logger.Information(Logs.InicioExecucaoServico, nameof(ConfirmaPagamentoMercadoPagoUseCase), nameof(ExecutarAsync));

            var pedido = await pedidoGateway.ObterPedidoComRelacionamentosAsync(idPedido, cancellationToken);
            if (pedido is null)
            {
                validationContext.AddValidation(string.Format(Textos.NaoEncontrado, nameof(Pedido)));
                return default;
            }

            var pagamento = await pagamentoGateway.CadastrarPedidoMercadoPagoAsync(pedido, cancellationToken);

            await pagamentoGateway.AdicionarPagamentoAsync(pagamento, cancellationToken);

            pedido = await pedidoGateway.ObterPedidoComRelacionamentosAsync(idPedido, cancellationToken);

            logger.Information(Logs.FimExecucaoServico, nameof(ConfirmaPagamentoMercadoPagoUseCase), nameof(ExecutarAsync), pedido);

            return pedido;
        }
        catch (Exception ex)
        {
            await unitOfWork.CommitAsync();
            logger.Error(ex, Logs.ErroGenerico, nameof(ConfirmaPagamentoMercadoPagoUseCase), nameof(ExecutarAsync));
            throw;
        }
    }
}
