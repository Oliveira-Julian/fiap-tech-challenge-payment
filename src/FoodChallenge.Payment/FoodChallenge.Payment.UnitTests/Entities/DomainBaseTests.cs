using FoodChallenge.Payment.Domain.Entities;

namespace FoodChallenge.Payment.UnitTests.Entities;

public class DomainBaseTests : TestBase
{
    private class TestDomainEntity : DomainBase
    {
    }

    [Fact]
    public void Construtor_DeveCriarEntidade_ComValoresPadrao()
    {
        var entidade = new TestDomainEntity();
        Assert.True(entidade.Ativo);
        Assert.Null(entidade.Id);
        Assert.Null(entidade.DataAtualizacao);
        Assert.Null(entidade.DataExclusao);
    }

    [Fact]
    public void Atualizar_DeveDefinirDataAtualizacao()
    {
        var entidade = new TestDomainEntity();
        entidade.Atualizar();
        Assert.NotNull(entidade.DataAtualizacao);
        Assert.True(entidade.DataAtualizacao <= DateTime.UtcNow);
        Assert.True(entidade.DataAtualizacao > DateTime.UtcNow.AddSeconds(-1));
    }

    [Fact]
    public void Excluir_DeveDesativarEntidade_EDefinirDataExclusaoEAtualizacao()
    {
        var entidade = new TestDomainEntity { Ativo = true };
        entidade.Excluir();
        Assert.False(entidade.Ativo);
        Assert.NotNull(entidade.DataExclusao);
        Assert.NotNull(entidade.DataAtualizacao);
        Assert.True(entidade.DataExclusao <= DateTime.UtcNow);
        Assert.True(entidade.DataAtualizacao <= DateTime.UtcNow);
    }
}
