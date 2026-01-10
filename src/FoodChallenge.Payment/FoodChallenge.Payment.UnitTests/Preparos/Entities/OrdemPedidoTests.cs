using Bogus;
using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Preparos;

namespace FoodChallenge.Payment.UnitTests.Preparos.Entities;

public class OrdemPedidoTests : TestBase
{
    private readonly Faker _faker;

    public OrdemPedidoTests()
    {
        _faker = GetFaker();
    }

    [Fact]
    public void Construtor_DeveCriarOrdemPedido_ComStatusPadrao()
    {
        var ordemPedido = new OrdemPedido();
        Assert.Equal(PreparoStatus.NaFilaParaPreparacao, ordemPedido.Status);
        Assert.Null(ordemPedido.DataInicioPreparacao);
        Assert.Null(ordemPedido.DataFimPreparacao);
    }

    [Fact]
    public void Cadastrar_DeveInicializarOrdemPedido_ComIdGerado()
    {
        var ordemPedido = new OrdemPedido();
        var idPedido = _faker.Random.Guid();
        ordemPedido.Cadastrar(idPedido);
        Assert.NotEqual(Guid.Empty, ordemPedido.Id.Value);
        Assert.Equal(idPedido, ordemPedido.IdPedido);
    }

    [Theory]
    [InlineData(PreparoStatus.NaFilaParaPreparacao, PreparoStatus.EmPreparacao, true)]
    [InlineData(PreparoStatus.EmPreparacao, PreparoStatus.Concluido, true)]
    [InlineData(PreparoStatus.NaFilaParaPreparacao, PreparoStatus.Concluido, false)]
    [InlineData(PreparoStatus.Concluido, PreparoStatus.EmPreparacao, false)]
    public void PermitirAlterarStatus_DeveRetornarCorreto_SegundoStatusAtualEProximo(
        PreparoStatus statusAtual, PreparoStatus proximoStatus, bool resultado)
    {
        var ordemPedido = new OrdemPedido { Status = statusAtual };
        var podeAlterar = ordemPedido.PermitirAlterarStatus(proximoStatus);
        Assert.Equal(resultado, podeAlterar);
    }

    [Fact]
    public void IniciarPreparacao_DeveAtualizarStatusEDefinirDataInicio()
    {
        var ordemPedido = new OrdemPedido();
        ordemPedido.IniciarPreparacao();
        Assert.Equal(PreparoStatus.EmPreparacao, ordemPedido.Status);
        Assert.NotNull(ordemPedido.DataInicioPreparacao);
        Assert.True(ordemPedido.DataInicioPreparacao <= DateTime.UtcNow);
        Assert.NotNull(ordemPedido.DataAtualizacao);
    }

    [Fact]
    public void FinalizarPreparacao_DeveAtualizarStatusEDefinirDataFim()
    {
        var ordemPedido = new OrdemPedido();
        ordemPedido.FinalizarPreparacao();
        Assert.Equal(PreparoStatus.Concluido, ordemPedido.Status);
        Assert.NotNull(ordemPedido.DataFimPreparacao);
        Assert.True(ordemPedido.DataFimPreparacao <= DateTime.UtcNow);
        Assert.NotNull(ordemPedido.DataAtualizacao);
    }
}
