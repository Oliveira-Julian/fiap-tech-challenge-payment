using FoodChallenge.Common.Entities;
using FoodChallenge.Common.Interfaces;
using FoodChallenge.Common.Validators;
using FoodChallenge.Infrastructure.Clients.MercadoPago.Clients;
using FoodChallenge.Infrastructure.Clients.MercadoPago.Settings;
using FoodChallenge.Infrastructure.Data.Postgres.EntityFramework.Repositories.Pedidos.Interfaces;
using FoodChallenge.Infrastructure.Data.Postgres.EntityFramework.Repositories.Preparos.Interfaces;
using FoodChallenge.Payment.Adapter.Gateways;
using FoodChallenge.Payment.Adapter.Mappers;
using FoodChallenge.Payment.Adapter.Presenters;
using FoodChallenge.Payment.Application.Pagamentos.Models.Requests;
using FoodChallenge.Payment.Application.Pagamentos.UseCases;
using FoodChallenge.Payment.Application.Pedidos.Models.Responses;
using FoodChallenge.Payment.Domain.Globalization;

namespace FoodChallenge.Payment.Adapter.Controllers;

public class PagamentoAppController(ValidationContext validationContext,
    IUnitOfWork unitOfWork,
    IPedidoRepository pedidoDataSource,
    IPedidoPagamentoRepository pagamentoDataSource,
    IOrdemPedidoRepository ordemPedidoDataSource,
    IMercadoPagoClient mercadoPagoClient,
    MercadoPagoSettings mercadoPagoSettings)
{
    public async Task<byte[]> ObterImagemQrCodeAsync(Guid idPedido, CancellationToken cancellationToken)
    {
        var pedidoGateway = new PedidoGateway(pedidoDataSource);
        var useCase = new ObtemImagemQrCodePagamentoUseCase(validationContext, pedidoGateway);

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

    public async Task<Resposta> ConfirmarPagamentoMercadoPagoAsync(WebhookMercadoPagoPagamentoRequest request, CancellationToken cancellationToken)
    {
        var pedidoGateway = new PedidoGateway(pedidoDataSource);
        var pagamentoGateway = new PagamentoGateway(pagamentoDataSource, mercadoPagoClient, mercadoPagoSettings);
        var ordemPedidoGateway = new OrdemPedidoGateway(ordemPedidoDataSource);
        var useCase = new ConfirmaPagamentoMercadoPagoUseCase(validationContext, unitOfWork, pagamentoGateway, pedidoGateway, ordemPedidoGateway);

        var notificacaoMercadoPago = PagamentoMapper.ToDomain(request);
        var pedido = await useCase.ExecutarAsync(notificacaoMercadoPago, cancellationToken);

        var response = PedidoPresenter.ToResponse(pedido);
        return Resposta.ComSucesso(string.Format(Textos.PagamentoRealizadoComSucesso, response?.Id));
    }
}
