using FoodChallenge.Common.Entities;
using FoodChallenge.Common.Validators;

namespace FoodChallenge.Payment.UnitTests.Common.Validators;

public class ValidationContextTests : TestBase
{
    [Fact]
    public void Construtor_DeveCriarContextoVazio()
    {
        var context = new ValidationContext();
        Assert.False(context.HasValidations);
        Assert.Empty(context.ValidationMessages);
    }

    [Fact]
    public void AddValidation_DeveAdicionarMensagem()
    {
        var context = new ValidationContext();
        var mensagem = "Erro de validação";
        context.AddValidation(mensagem);
        Assert.True(context.HasValidations);
        Assert.Single(context.ValidationMessages);
        Assert.Contains(mensagem, context.ValidationMessages);
    }

    [Fact]
    public void AddValidation_DeveRetornarProprioContexto()
    {
        var context = new ValidationContext();
        var resultado = context.AddValidation("Mensagem");
        Assert.Same(context, resultado);
    }

    [Fact]
    public void AddValidations_DeveAdicionarMultiplasMensagens()
    {
        var context = new ValidationContext();
        var mensagens = new List<string> { "Erro 1", "Erro 2", "Erro 3" };
        context.AddValidations(mensagens);
        Assert.True(context.HasValidations);
        Assert.Equal(3, context.ValidationMessages.Count);
        Assert.All(mensagens, m => Assert.Contains(m, context.ValidationMessages));
    }

    [Fact]
    public void AddValidations_ComResponseValidation_DeveAdicionarApenasErros()
    {
        var context = new ValidationContext();
        var respostas = new List<Resposta>
        {
            Resposta.ComSucesso(),
            Resposta.ComFalha(new List<string> { "Erro 1", "Erro 2" })
        };
        var responseValidation = new ResponseValidation(respostas);
        context.AddValidations(responseValidation);
        Assert.True(context.HasValidations);
        Assert.Equal(2, context.ValidationMessages.Count);
        Assert.Contains("Erro 1", context.ValidationMessages);
        Assert.Contains("Erro 2", context.ValidationMessages);
    }

    [Fact]
    public void HasValidations_DeveRetornarFalse_QuandoSemMensagens()
    {
        var context = new ValidationContext();
        Assert.False(context.HasValidations);
    }

    [Fact]
    public void HasValidations_DeveRetornarTrue_QuandoComMensagens()
    {
        var context = new ValidationContext();
        context.AddValidation("Erro");
        Assert.True(context.HasValidations);
    }
}
