using FoodChallenge.Common.Interfaces;
using FoodChallenge.Payment.Application.Pagamentos.Interfaces;
using FoodChallenge.Payment.Application.Pagamentos.Models.Requests;
using FoodChallenge.Payment.Domain.Constants;
using FoodChallenge.Payment.Domain.Pagamentos;
using Serilog;

namespace FoodChallenge.Payment.Application.Pagamentos.UseCases;

public sealed class CriaPagamentoUseCase(
    IUnitOfWork unitOfWork,
    IPagamentoGateway pagamentoGateway) : ICriaPagamentoUseCase
{
    private readonly ILogger logger = Log.ForContext<CriaPagamentoUseCase>();

    public async Task<Pagamento> ExecutarAsync(CriarPagamentoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            logger.Information(Logs.InicioExecucaoServico, nameof(CriaPagamentoUseCase), nameof(ExecutarAsync));

            var pagamento = await pagamentoGateway.CriarPagamentoAsync(request, cancellationToken);

            await pagamentoGateway.AdicionarPagamentoAsync(pagamento, cancellationToken);

            logger.Information(Logs.FimExecucaoServico, nameof(CriaPagamentoUseCase), nameof(ExecutarAsync), pagamento);

            return pagamento;
        }
        catch (Exception ex)
        {
            await unitOfWork.CommitAsync();
            logger.Error(ex, Logs.ErroGenerico, nameof(CriaPagamentoUseCase), nameof(ExecutarAsync));
            throw;
        }
    }
}
