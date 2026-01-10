using FoodChallenge.Payment.Domain.Extensions;

namespace FoodChallenge.Payment.UnitTests.Extensions;

public class StringExtensionsTests : TestBase
{
    [Fact]
    public void GetRandomCode_DeveGerarCodigoComTamanhoPadrao()
    {
        var codigo = StringExtensions.GetRandomCode();
        Assert.NotNull(codigo);
        Assert.Equal(6, codigo.Length);
        Assert.All(codigo, c => Assert.True(char.IsLetterOrDigit(c)));
    }

    [Theory]
    [InlineData(3)]
    [InlineData(8)]
    [InlineData(10)]
    public void GetRandomCode_DeveGerarCodigoComTamanhoEspecificado(int tamanho)
    {
        var codigo = StringExtensions.GetRandomCode(tamanho);
        Assert.NotNull(codigo);
        Assert.Equal(tamanho, codigo.Length);
        Assert.All(codigo, c => Assert.True(char.IsLetterOrDigit(c)));
    }

    [Fact]
    public void GetRandomCode_DeveGerarCodigosDiferentes()
    {
        var codigo1 = StringExtensions.GetRandomCode();
        var codigo2 = StringExtensions.GetRandomCode();
        Assert.NotEqual(codigo1, codigo2);
    }

    [Fact]
    public void GetRandomCode_DeveSomenteConterCaracteresPermitidos()
    {
        const string caracteresPermitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var codigo = StringExtensions.GetRandomCode(20);
        Assert.All(codigo, c => Assert.Contains(c, caracteresPermitidos));
    }
}
