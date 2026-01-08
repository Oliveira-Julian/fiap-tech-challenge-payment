using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Pagamentos;

namespace FoodChallenge.Payment.UnitTests.Pagamentos.Entities;

public class PagamentoPropertiesTests : TestBase
{
    [Fact]
    public void Pagamento_DevePermitirDefinirTodasPropriedades()
    {
        var idPedido = Guid.NewGuid();
        var chaveMercadoPago = Guid.NewGuid();
        var pagamento = new Pagamento
        {
            IdPedido = idPedido,
            ChaveMercadoPagoOrdem = chaveMercadoPago,
            IdMercadoPagoOrdem = "ordem-123",
            IdMercadoPagoPagamento = "pagamento-456",
            Valor = 100.50m,
            Metodo = PagamentoMetodo.Pix,
            QrCode = "qrcode-string"
        };
        Assert.Equal(idPedido, pagamento.IdPedido);
        Assert.Equal(chaveMercadoPago, pagamento.ChaveMercadoPagoOrdem);
        Assert.Equal("ordem-123", pagamento.IdMercadoPagoOrdem);
        Assert.Equal("pagamento-456", pagamento.IdMercadoPagoPagamento);
        Assert.Equal(100.50m, pagamento.Valor);
        Assert.Equal(PagamentoMetodo.Pix, pagamento.Metodo);
        Assert.Equal("qrcode-string", pagamento.QrCode);
    }
}
