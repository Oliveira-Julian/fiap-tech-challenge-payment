using FoodChallenge.Common.Entities;
using FoodChallenge.Common.Interfaces;
using FoodChallenge.Common.Validators;
using FoodChallenge.Infrastructure.Clients.MercadoPago.Clients;
using FoodChallenge.Infrastructure.Clients.MercadoPago.Settings;
using FoodChallenge.Infrastructure.Data.Mongo.Repositories.Pedidos.Interfaces;
using FoodChallenge.Payment.Adapter.Gateways;
using FoodChallenge.Payment.Adapter.Mappers;
using FoodChallenge.Payment.Adapter.Presenters;
using FoodChallenge.Payment.Application.Pagamentos.Models.Requests;
using FoodChallenge.Payment.Application.Pagamentos.Models.Responses;
using FoodChallenge.Payment.Application.Pagamentos.UseCases;
using FoodChallenge.Payment.Application.Pedidos.Models.Responses;
using FoodChallenge.Payment.Domain.Globalization;

namespace FoodChallenge.Payment.Adapter.Controllers;

public class PagamentoAppController(ValidationContext validationContext,
    IUnitOfWork unitOfWork,
    IPedidoRepository pedidoDataSource,
    IPedidoPagamentoRepository pagamentoDataSource,
    IMercadoPagoClient mercadoPagoClient,
    MercadoPagoSettings mercadoPagoSettings)
{
    public async Task<byte[]> ObterImagemQrCodeAsync(Guid idPedido, CancellationToken cancellationToken)
    {
        var pagamentoGateway = new PagamentoGateway(pagamentoDataSource, mercadoPagoClient, mercadoPagoSettings);
        var useCase = new ObtemImagemQrCodePagamentoUseCase(validationContext, pagamentoGateway);

        return await useCase.ExecutarAsync(idPedido, cancellationToken);
    }

    public async Task<Resposta> ObterQrCodePagamentoAsync(Guid idPedido, CancellationToken cancellationToken)
    {
        var pedidoGateway = new PedidoGateway(pedidoDataSource);
        var pagamentoGateway = new PagamentoGateway(pagamentoDataSource, mercadoPagoClient, mercadoPagoSettings);
        var useCase = new GeraQrCodePagamentoUseCase(validationContext, unitOfWork, pedidoGateway, pagamentoGateway);

        var pedido = await useCase.ExecutarAsync(idPedido, cancellationToken);
        return Resposta<PedidoResponse>.ComSucesso(PedidoPresenter.ToResponse(pedido));
    }

    //TODO - Criar no MS Order para atualizar o status do pedido e criar a ordem do pedido
    public async Task<Resposta> ConfirmarPagamentoMercadoPagoAsync(WebhookMercadoPagoPagamentoRequest request, CancellationToken cancellationToken)
    {
        // var pedidoGateway = new PedidoGateway(pedidoDataSource);
        var pagamentoGateway = new PagamentoGateway(pagamentoDataSource, mercadoPagoClient, mercadoPagoSettings);
        // var ordemPedidoGateway = new OrdemPedidoGateway(ordemPedidoDataSource);
        var useCase = new ConfirmaPagamentoMercadoPagoUseCase(validationContext, unitOfWork, pagamentoGateway);

        var notificacaoMercadoPago = PagamentoMapper.ToDomain(request);
        try
        {
            var pagamento = await useCase.ExecutarAsync(notificacaoMercadoPago, cancellationToken);

            var response = PagamentoPresenter.ToResponse(pagamento);
            return Resposta<PagamentoResponse>.ComSucesso(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task<Resposta> CriarPagamentoAsync(CriarPagamentoRequest request, CancellationToken cancellationToken)
    {
        var pagamentoGateway = new PagamentoGateway(pagamentoDataSource, mercadoPagoClient, mercadoPagoSettings);
        var useCase = new CriaPagamentoUseCase(unitOfWork, pagamentoGateway);

        var pagamento = await useCase.ExecutarAsync(request, cancellationToken);

        var response = PagamentoPresenter.ToResponse(pagamento);
        return Resposta<PagamentoResponse>.ComSucesso(response);
    }
}
