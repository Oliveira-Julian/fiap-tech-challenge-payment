using FoodChallenge.Payment.Domain.Clientes.ValueObjects;

namespace FoodChallenge.Payment.UnitTests.Clientes.ValueObjects;

public class CpfTests : TestBase
{
    [Theory]
    [InlineData("11111111111")]
    [InlineData("22222222222")]
    [InlineData("00000000000")]
    [InlineData("12345678900")]
    public void EhValido_DeveRetornarFalse_QuandoCpfInvalido(string cpfInvalido)
    {
        var cpf = new Cpf(cpfInvalido);
        var ehValido = cpf.EhValido();
        Assert.False(ehValido);
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("123")]
    [InlineData("")]
    public void EhValido_DeveRetornarFalse_QuandoTamanhoInvalido(string cpfInvalido)
    {
        var cpf = new Cpf(cpfInvalido);
        var ehValido = cpf.EhValido();
        Assert.False(ehValido);
    }

    [Theory]
    [InlineData("111.222.333-96", "11122233396")]
    [InlineData("123-456-789.00", "12345678900")]
    public void RemoverMascara_DeveRemoverCaracteresEspeciais(string cpfComMascara, string cpfEsperado)
    {
        var cpfSemMascara = Cpf.RemoverMascara(cpfComMascara);
        Assert.Equal(cpfEsperado, cpfSemMascara);
    }

    [Fact]
    public void RemoverMascara_DeveRetornarStringVazia_QuandoNull()
    {
        var cpfSemMascara = Cpf.RemoverMascara(null);
        Assert.Equal(string.Empty, cpfSemMascara);
    }

    [Theory]
    [InlineData("12345678909", "123.456.789-09")]
    public void ToString_DeveFormatarComMascara(string cpf, string expected)
    {
        var cpfObj = new Cpf(cpf);
        var resultado = cpfObj.ToString();
        Assert.Equal(expected, resultado);
    }

    [Fact]
    public void ToString_DeveRetornarStringVazia_QuandoValorVazio()
    {
        var cpf = new Cpf("");
        var resultado = cpf.ToString();
        Assert.Equal(string.Empty, resultado);
    }

    [Fact]
    public void Equals_DeveRetornarTrue_QuandoCpfsIguais()
    {
        var cpf1 = new Cpf("12345678909");
        var cpf2 = new Cpf("12345678909");
        var resultado = cpf1.Equals(cpf2);
        Assert.True(resultado);
    }

    [Fact]
    public void Equals_DeveRetornarFalse_QuandoCpfsDiferentes()
    {
        var cpf1 = new Cpf("12345678909");
        var cpf2 = new Cpf("98765432100");
        var resultado = cpf1.Equals(cpf2);
        Assert.False(resultado);
    }

    [Fact]
    public void GetHashCode_DeveRetornarHashCode_DoValor()
    {
        var cpf = new Cpf("12345678909");
        var hashCode = cpf.GetHashCode();
        Assert.Equal("12345678909".GetHashCode(), hashCode);
    }

    [Fact]
    public void Construtor_DeveLimparMascara_AoCriarCpf()
    {
        var cpf = new Cpf("123.456.789-09");
        Assert.Equal("12345678909", cpf.Valor);
    }

    [Fact]
    public void Construtor_DeveRetornarStringVazia_QuandoValorNull()
    {
        var cpf = new Cpf(null);
        Assert.Equal(string.Empty, cpf.Valor);
    }
}
