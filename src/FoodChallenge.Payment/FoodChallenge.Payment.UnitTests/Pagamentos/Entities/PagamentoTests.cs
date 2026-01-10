using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Pagamentos;

namespace FoodChallenge.Payment.UnitTests.Pagamentos.Entities;

public class PagamentoTests : TestBase
{
    [Fact]
    public void Cadastrar_DeveCriarNovoPagamento_ComStatusPendenteEIdGerado()
    {
        var pagamento = new Pagamento();
        pagamento.Cadastrar();
        Assert.NotEqual(Guid.Empty, pagamento.Id);
        Assert.Equal(PagamentoStatus.Pendente, pagamento.Status);
        Assert.True(pagamento.DataCriacao <= DateTime.UtcNow);
        Assert.True(pagamento.DataCriacao > DateTime.UtcNow.AddSeconds(-1));
    }

    [Theory]
    [InlineData(PagamentoStatus.Aprovado)]
    [InlineData(PagamentoStatus.Recusado)]
    [InlineData(PagamentoStatus.Pendente)]
    [InlineData(PagamentoStatus.NaoIdentificado)]
    public void AtualizarStatus_DeveAtualizarStatusDoPagamento(PagamentoStatus novoStatus)
    {
        var pagamento = new Pagamento();
        pagamento.Cadastrar();
        var dataAtualizacaoAnterior = pagamento.DataAtualizacao;
        pagamento.AtualizarStatus(novoStatus);
        Assert.Equal(novoStatus, pagamento.Status);
        Assert.NotNull(pagamento.DataAtualizacao);
        Assert.NotEqual(dataAtualizacaoAnterior, pagamento.DataAtualizacao);
    }

    [Fact]
    public void AtualizarStatus_DeveAtualizarDataAtualizacao()
    {
        var pagamento = new Pagamento();
        pagamento.Cadastrar();
        pagamento.AtualizarStatus(PagamentoStatus.Aprovado);
        Assert.NotNull(pagamento.DataAtualizacao);
        Assert.True(pagamento.DataAtualizacao <= DateTime.UtcNow);
    }
}
