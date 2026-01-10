using FoodChallenge.Common.Entities;
using FoodChallenge.Common.Validators;

namespace FoodChallenge.Payment.UnitTests.Common.Validators;

public class ResponseValidationTests : TestBase
{
    [Fact]
    public void Valid_DeveRetornarTrue_QuandoNenhumErro()
    {
        var respostas = new List<Resposta>
        {
            Resposta.ComSucesso()
        };
        var response = new ResponseValidation(respostas);
        Assert.True(response.Valid);
    }

    [Fact]
    public void Valid_DeveRetornarFalse_QuandoExistemErros()
    {
        var respostas = new List<Resposta>
        {
            Resposta.ComFalha(new List<string> { "Erro" })
        };
        var response = new ResponseValidation(respostas);
        Assert.False(response.Valid);
    }
}
