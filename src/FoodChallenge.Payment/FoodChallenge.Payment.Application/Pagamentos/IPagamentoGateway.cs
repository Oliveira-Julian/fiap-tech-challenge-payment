using FoodChallenge.Payment.Domain.Pagamentos;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Application.Pagamentos;

public interface IPagamentoGateway
{
    Task AdicionarPagamentoAsync(Pagamento pagamento, CancellationToken cancellationToken);
    void AtualizarPagamento(Pagamento pagamento);
    Task<Pagamento> ObterPedidoMercadoPagoAsync(string idPedidoMercadoPago, CancellationToken cancellationToken);
    Task<Pagamento> CadastrarPedidoMercadoPagoAsync(Pedido pedido, CancellationToken cancellationToken);
    Task<Pagamento> ObterPagamentoIdMercadoPagoAsync(string idMercadoPagoPagamento, CancellationToken cancellationToken);
}
