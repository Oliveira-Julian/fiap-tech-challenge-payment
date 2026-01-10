using FoodChallenge.Common.Validators;

namespace FoodChallenge.Payment.UnitTests.Common.Validators;

public class ResultValidationTests : TestBase
{
    [Fact]
    public void Construtor_DeveCriarComMensagensVazias_EIsValidTrue()
    {
        var mensagens = new List<string>();
        var result = new ResultValidation(mensagens);
        Assert.True(result.IsValid);
        Assert.Empty(result.Messages);
    }

    [Fact]
    public void Construtor_DeveCriarComMensagens_EIsValidFalse()
    {
        var mensagens = new List<string> { "Erro 1" };
        var result = new ResultValidation(mensagens);
        Assert.False(result.IsValid);
        Assert.Equal(mensagens, result.Messages);
    }
}
