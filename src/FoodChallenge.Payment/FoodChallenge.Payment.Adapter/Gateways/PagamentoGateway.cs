using FoodChallenge.Infrastructure.Clients.MercadoPago.Clients;
using FoodChallenge.Infrastructure.Clients.MercadoPago.Settings;
using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Pedidos.Interfaces;
using FoodChallenge.Payment.Adapter.Mappers;
using FoodChallenge.Payment.Application.Pagamentos;
using FoodChallenge.Payment.Application.Pagamentos.Models.Requests;
using FoodChallenge.Payment.Domain.Globalization;
using FoodChallenge.Payment.Domain.Pagamentos;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Adapter.Gateways;

public class PagamentoGateway : IPagamentoGateway
{
    private readonly IPedidoPagamentoRepository pagamentoRepository;
    private readonly IMercadoPagoClient mercadoPagoClient;
    private readonly MercadoPagoSettings mercadoPagoSettings;

    public PagamentoGateway(
        IPedidoPagamentoRepository pagamentoRepository,
        IMercadoPagoClient mercadoPagoClient,
        MercadoPagoSettings mercadoPagoSettings)
    {
        this.pagamentoRepository = pagamentoRepository;
        this.mercadoPagoClient = mercadoPagoClient;
        this.mercadoPagoSettings = mercadoPagoSettings;
    }

    public async Task AdicionarPagamentoAsync(Pagamento pagamento, CancellationToken cancellationToken)
    {
        var pagamentoEntity = PagamentoMapper.ToEntity(pagamento);

        await pagamentoRepository.AddAsync(pagamentoEntity, cancellationToken);
    }

    public void AtualizarPagamento(Pagamento pagamento)
    {
        var pagamentoEntity = PagamentoMapper.ToEntity(pagamento);
        pagamentoRepository.Update(pagamentoEntity);
    }

    public async Task<Pagamento> ObterPagamentoIdMercadoPagoAsync(string idMercadoPagoPagamento, CancellationToken cancellationToken)
    {
        var pagamentoEntity = await pagamentoRepository.GetByIdMercadoPagoPagamentoAsync(idMercadoPagoPagamento, cancellationToken);

        return PagamentoMapper.ToDomain(pagamentoEntity);
    }

    public async Task<Pagamento> CadastrarPedidoMercadoPagoAsync(Pedido pedido, CancellationToken cancellationToken)
    {
        var ordemId = Guid.NewGuid();
        var request = MercadoPagoOrderMapper.ToRequest(pedido, mercadoPagoSettings);

        var response = await mercadoPagoClient.CadastrarOrdemAsync(ordemId, request, cancellationToken);

        if (response is null)
            throw new Exception(Textos.ErroInesperado);

        var pagamento = MercadoPagoOrderMapper.ToDomain(response, ordemId, pedido.Id.Value);

        return pagamento;
    }

    public async Task<Pagamento> ObterPedidoMercadoPagoAsync(string idPedidoMercadoPago, CancellationToken cancellationToken)
    {
        var response = await mercadoPagoClient.ObterOrdemAsync(idPedidoMercadoPago, cancellationToken);

        if (response is null)
            throw new Exception(Textos.ErroInesperado);

        var pagamento = MercadoPagoOrderMapper.ToDomain(response);

        return pagamento;
    }

    public async Task<Pagamento> CriarPagamentoAsync(CriarPagamentoRequest request, CancellationToken cancellationToken)
    {
        var ordemId = Guid.NewGuid();
        var mercadoPagoRequest = MercadoPagoOrderMapper.ToRequest(request, mercadoPagoSettings);

        var response = await mercadoPagoClient.CadastrarOrdemAsync(ordemId, mercadoPagoRequest, cancellationToken);

        if (response is null)
            throw new Exception(Textos.ErroInesperado);
        
        var pagamento = MercadoPagoOrderMapper.ToDomain(response, ordemId, request.IdPedido); 
        return pagamento;
    }
}
