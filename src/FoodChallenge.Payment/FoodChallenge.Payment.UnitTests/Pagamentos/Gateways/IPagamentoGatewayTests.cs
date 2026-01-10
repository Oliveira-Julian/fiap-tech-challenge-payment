using Bogus;
using FoodChallenge.Payment.Application.Pagamentos;
using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Pagamentos;
using FoodChallenge.Payment.UnitTests.Mocks;
using Moq;

namespace FoodChallenge.Payment.UnitTests.Pagamentos.Gateways;

public class IPagamentoGatewayTests : TestBase
{
    private readonly Faker _faker;
    private readonly Mock<IPagamentoGateway> _pagamentoGateway;

    public IPagamentoGatewayTests()
    {
        _faker = GetFaker();
        _pagamentoGateway = new Mock<IPagamentoGateway>();
    }

    [Fact]
    public async Task AdicionarPagamentoAsync_DeveSerChamado()
    {
        var pagamento = PagamentoMock.CriarValido();
        _pagamentoGateway
            .Setup(g => g.AdicionarPagamentoAsync(It.IsAny<Pagamento>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        await _pagamentoGateway.Object.AdicionarPagamentoAsync(pagamento, CancellationToken.None);
        _pagamentoGateway.Verify(g => g.AdicionarPagamentoAsync(pagamento, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterPagamentoIdMercadoPagoAsync_DeveRetornarPagamento()
    {
        var idMercadoPago = _faker.Random.Guid().ToString();
        var pagamento = PagamentoMock.CriarValido();
        pagamento.IdMercadoPagoPagamento = idMercadoPago;

        _pagamentoGateway
            .Setup(g => g.ObterPagamentoIdMercadoPagoAsync(idMercadoPago, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagamento);
        var resultado = await _pagamentoGateway.Object.ObterPagamentoIdMercadoPagoAsync(idMercadoPago, CancellationToken.None);
        Assert.NotNull(resultado);
        Assert.Equal(idMercadoPago, resultado.IdMercadoPagoPagamento);
    }
}
