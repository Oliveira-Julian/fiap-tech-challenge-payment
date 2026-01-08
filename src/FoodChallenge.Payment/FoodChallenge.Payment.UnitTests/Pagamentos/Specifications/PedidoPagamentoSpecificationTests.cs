using FoodChallenge.Common.Extensions;
using FoodChallenge.Payment.Application.Pagamentos.Specifications;
using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Globalization;
using FoodChallenge.Payment.UnitTests.Mocks;

namespace FoodChallenge.Payment.UnitTests.Pagamentos.Specifications;

public class PedidoPagamentoSpecificationTests : TestBase
{
    private readonly PedidoPagamentoSpecification _specification;

    public PedidoPagamentoSpecificationTests()
    {
        _specification = new PedidoPagamentoSpecification();
    }

    [Theory]
    [InlineData(PedidoStatus.Recebido)]
    public async Task DeveSerValida_QuandoStatusPermitePagamento(PedidoStatus status)
    {
        var pedido = PedidoMock.CriarComStatus(status: status);
        var result = await _specification.ValidateModelAsync(pedido, CancellationToken.None);
        Assert.True(result.Valid);
        Assert.Empty(result.Responses);
    }

    [Theory]
    [InlineData(PedidoStatus.NaFila)]
    [InlineData(PedidoStatus.EmPreparacao)]
    [InlineData(PedidoStatus.AguardandoRetirada)]
    [InlineData(PedidoStatus.Finalizado)]
    public async Task DeveSerInvalida_QuandoStatusNaoPermitePagamento(PedidoStatus status)
    {
        var pedido = PedidoMock.CriarComStatus(status: status);

        var validationMessages = new List<string>
        {
            string.Format(Textos.PagamentoStatusNaoPermitido, status.GetDescription())
        };
        var result = await _specification.ValidateModelAsync(pedido, CancellationToken.None);
        Assert.False(result.Valid);
        Assert.Equal(validationMessages, result.Responses.SelectMany(r => r.Mensagens));
    }
}
