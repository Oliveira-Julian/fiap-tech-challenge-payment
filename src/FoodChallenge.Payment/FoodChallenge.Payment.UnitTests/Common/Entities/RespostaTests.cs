using FoodChallenge.Common.Entities;

namespace FoodChallenge.Payment.UnitTests.Common.Entities;

public class RespostaTests : TestBase
{
    [Fact]
    public void ComSucesso_SemMensagens_DeveCriarRespostaComSucesso()
    {
        var resposta = Resposta.ComSucesso();
        Assert.True(resposta.Sucesso);
        Assert.Null(resposta.Mensagens);
    }

    [Fact]
    public void ComSucesso_ComMensagens_DeveCriarRespostaComSucesso()
    {
        var mensagens = new List<string> { "Mensagem 1", "Mensagem 2" };
        var resposta = Resposta.ComSucesso(mensagens);
        Assert.True(resposta.Sucesso);
        Assert.Equal(mensagens, resposta.Mensagens);
    }

    [Fact]
    public void ComSucesso_ComMensagemUnica_DeveCriarRespostaComSucesso()
    {
        var mensagem = "Operação realizada com sucesso";
        var resposta = Resposta.ComSucesso(mensagem);
        Assert.True(resposta.Sucesso);
        Assert.Contains(mensagem, resposta.Mensagens);
    }

    [Fact]
    public void ComFalha_ComMensagens_DeveCriarRespostaComFalha()
    {
        var mensagens = new List<string> { "Erro 1", "Erro 2" };
        var resposta = Resposta.ComFalha(mensagens);
        Assert.False(resposta.Sucesso);
        Assert.Equal(mensagens, resposta.Mensagens);
    }

    [Fact]
    public void ComFalha_ComMensagemUnica_DeveCriarRespostaComFalha()
    {
        var mensagem = "Erro na operação";
        var resposta = Resposta.ComFalha(mensagem);
        Assert.False(resposta.Sucesso);
        Assert.Contains(mensagem, resposta.Mensagens);
    }
}
