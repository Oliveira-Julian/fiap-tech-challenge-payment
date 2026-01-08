using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Pagamentos;

namespace FoodChallenge.Payment.UnitTests.Pagamentos.Entities;

public class NotificacaoMercadoPagoTests : TestBase
{
    [Theory]
    [InlineData("payment.processed", true)]
    [InlineData("payment.canceled", true)]
    [InlineData("payment.created", true)]
    [InlineData("payment.expired", true)]
    [InlineData("payment.refunded", true)]
    public void VerificarAcaoEhValida_DeveRetornarTrue_QuandoAcaoValida(string acao, bool esperado)
    {
        var notificacao = new NotificacaoMercadoPago { Acao = acao };
        var resultado = notificacao.VerificarAcaoEhValida();
        Assert.Equal(esperado, resultado);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("payment.invalid")]
    [InlineData("invalid.action")]
    public void VerificarAcaoEhValida_DeveRetornarFalse_QuandoAcaoInvalida(string acao)
    {
        var notificacao = new NotificacaoMercadoPago { Acao = acao };
        var resultado = notificacao.VerificarAcaoEhValida();
        Assert.False(resultado);
    }

    [Theory]
    [InlineData("processed", PagamentoStatus.Aprovado)]
    [InlineData("canceled", PagamentoStatus.Recusado)]
    [InlineData("expired", PagamentoStatus.Recusado)]
    [InlineData("refunded", PagamentoStatus.Recusado)]
    [InlineData("created", PagamentoStatus.Pendente)]
    [InlineData("invalid", PagamentoStatus.NaoIdentificado)]
    [InlineData("", PagamentoStatus.NaoIdentificado)]
    public void ObterStatusPagamento_DeveRetornarStatusCorreto(string status, PagamentoStatus esperado)
    {
        var resultado = NotificacaoMercadoPago.ObterStatusPagamento(status);
        Assert.Equal(esperado, resultado);
    }

    [Theory]
    [InlineData("payment.processed", PagamentoStatus.Aprovado)]
    [InlineData("payment.canceled", PagamentoStatus.Recusado)]
    [InlineData("payment.created", PagamentoStatus.Pendente)]
    [InlineData("payment.expired", PagamentoStatus.Recusado)]
    [InlineData("payment.refunded", PagamentoStatus.Recusado)]
    public void Status_DeveRetornarStatusCorreto_BasedoNaAcao(string acao, PagamentoStatus esperado)
    {
        var notificacao = new NotificacaoMercadoPago { Acao = acao };
        var status = notificacao.Status;
        Assert.Equal(esperado, status);
    }
}
