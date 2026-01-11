using FoodChallenge.Common.Interfaces;
using FoodChallenge.Common.Validators;
using FoodChallenge.Payment.Application.Pagamentos.Interfaces;
using FoodChallenge.Payment.Application.Pagamentos.Specifications;
using FoodChallenge.Payment.Application.Pedidos;
using FoodChallenge.Payment.Application.Preparos;
using FoodChallenge.Payment.Domain.Constants;
using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Globalization;
using FoodChallenge.Payment.Domain.Pagamentos;
using FoodChallenge.Payment.Domain.Pedidos;
using FoodChallenge.Payment.Domain.Preparos;
using Serilog;

namespace FoodChallenge.Payment.Application.Pagamentos.UseCases;

public sealed class ConfirmaPagamentoMercadoPagoUseCase(
    ValidationContext validationContext,
    IUnitOfWork unitOfWork,
    IPagamentoGateway pagamentoGateway) : IConfirmaPagamentoMercadoPagoUseCase
{
    private readonly ILogger logger = Log.ForContext<ConfirmaPagamentoMercadoPagoUseCase>();

    public async Task<Pagamento> ExecutarAsync(NotificacaoMercadoPago notificacaoMercadoPago, CancellationToken cancellationToken)
    {
        logger.Information(Logs.InicioExecucaoServico, nameof(ConfirmaPagamentoMercadoPagoUseCase), nameof(ExecutarAsync));

        try
        {
            if (notificacaoMercadoPago.Tipo != "payment" || !notificacaoMercadoPago.VerificarAcaoEhValida())
            {
                validationContext.AddValidation(Textos.WebhookAcaoOuTipoNaoPermitido);
                return default;
            }

            var pagamento = await pagamentoGateway.ObterPagamentoIdMercadoPagoAsync(notificacaoMercadoPago.Id, cancellationToken);
            if (pagamento is null || !pagamento.IdPedido.HasValue)
            {
                validationContext.AddValidation(string.Format(Textos.NaoEncontrado, nameof(Pagamento)));
                return default;
            }

            var pagamentoMercadoPago = await pagamentoGateway.ObterPedidoMercadoPagoAsync(pagamento.IdMercadoPagoOrdem, cancellationToken);

            if (pagamentoMercadoPago is null || string.IsNullOrEmpty(pagamentoMercadoPago.IdMercadoPagoPagamento))
            {
                validationContext.AddValidation(string.Format(Textos.NaoEncontrado, nameof(Pagamento)));
                return default;
            }

            // var pedido = await pedidoGateway.ObterPedidoComRelacionamentosAsync(pagamento.IdPedido.Value, cancellationToken);
            // if (pedido is null)
            // {
            //     validationContext.AddValidation(string.Format(Textos.NaoEncontrado, nameof(Pedido)));
            //     return default;
            // }

            // validationContext.AddValidations(pedido, new PedidoPagamentoSpecification());

            // if (validationContext.HasValidations)
            //     return default;

            pagamento.AtualizarStatus(notificacaoMercadoPago.Status);
            pagamentoGateway.AtualizarPagamento(pagamento);

            
            //TODO -  Migrar para o MS Order, criar um endpoint para atualizar o status do pedido e criar a ordem do pedido, 
            // o MS Payment não deve ter essa responsabilidade e deve chamar a api do MS Order para essas atualizações 
            
            // if (notificacaoMercadoPago.Status != PagamentoStatus.Aprovado)
            //     return pedido;

            // pedido.AtualizarStatusPago();

            // pedidoGateway.AtualizarPedido(pedido);

            // var ordemPedido = new OrdemPedido();
            // ordemPedido.Cadastrar(pedido.Id.Value);
            // await ordemPedidoGateway.CadastrarOrdemPedidoAsync(ordemPedido, cancellationToken);
            
            logger.Information(Logs.FimExecucaoServico, nameof(ConfirmaPagamentoMercadoPagoUseCase), nameof(ExecutarAsync), pagamento);

            return pagamento;

        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.Error(ex, Logs.ErroGenerico, nameof(ConfirmaPagamentoMercadoPagoUseCase), nameof(ExecutarAsync));
            throw;
        }
    }
}
