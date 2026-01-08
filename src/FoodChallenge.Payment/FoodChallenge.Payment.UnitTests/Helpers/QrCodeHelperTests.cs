using FoodChallenge.Payment.Application.Helpers;

namespace FoodChallenge.Payment.UnitTests.Helpers;

public class QrCodeHelperTests : TestBase
{
    [Fact]
    public void GerarImagem_DeveRetornarArrayDeBytes_QuandoTextoValido()
    {
        var texto = "00020126580014BR.GOV.BCB.PIX0136123e4567-e12b-12d1-a456-426655440000";
        var resultado = QrCodeHelper.GerarImagem(texto);
        Assert.NotNull(resultado);
        Assert.NotEmpty(resultado);
        Assert.True(resultado.Length > 0);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void GerarImagem_DeveRetornarDefault_QuandoTextoInvalido(string textoInvalido)
    {
        var resultado = QrCodeHelper.GerarImagem(textoInvalido);
        Assert.Null(resultado);
    }

    [Fact]
    public void GerarImagem_DeveGerarImagensDiferentes_ParaTextoDiferente()
    {
        var texto1 = "texto1";
        var texto2 = "texto2";
        var resultado1 = QrCodeHelper.GerarImagem(texto1);
        var resultado2 = QrCodeHelper.GerarImagem(texto2);
        Assert.NotNull(resultado1);
        Assert.NotNull(resultado2);
        Assert.NotEqual(resultado1, resultado2);
    }

    [Fact]
    public void GerarImagem_DeveGerarImagensIguais_ParaMesmoTexto()
    {
        var texto = "mesmo-texto";
        var resultado1 = QrCodeHelper.GerarImagem(texto);
        var resultado2 = QrCodeHelper.GerarImagem(texto);
        Assert.NotNull(resultado1);
        Assert.NotNull(resultado2);
        Assert.Equal(resultado1, resultado2);
    }
}
