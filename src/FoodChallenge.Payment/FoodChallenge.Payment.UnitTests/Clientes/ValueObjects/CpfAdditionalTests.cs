using Bogus;
using FoodChallenge.Payment.Domain.Clientes.ValueObjects;

namespace FoodChallenge.Payment.UnitTests.Clientes.ValueObjects;

public class CpfAdditionalTests : TestBase
{
    private readonly Faker _faker;

    public CpfAdditionalTests()
    {
        _faker = GetFaker();
    }

    [Fact]
    public void GetHashCode_DeveRetornarHashCode_QuandoValorNull()
    {
        var cpf = new Cpf(null);
        var hashCode = cpf.GetHashCode();
        Assert.NotEqual(0, hashCode);
    }

    [Fact]
    public void Equals_DeveRetornarFalse_QuandoCompararComObjetoNulo()
    {
        var cpf = new Cpf("12345678909");
        var resultado = cpf.Equals(null);
        Assert.False(resultado);
    }

    [Fact]
    public void Equals_DeveRetornarFalse_QuandoCompararComObjetoDiferenteTipo()
    {
        var cpf = new Cpf("12345678909");
        var objeto = "string qualquer";
        var resultado = cpf.Equals(objeto);
        Assert.False(resultado);
    }

    [Fact]
    public void Construtor_DeveRetornarStringVazia_QuandoStringVazia()
    {
        var cpf = new Cpf("");
        Assert.Equal(string.Empty, cpf.Valor);
    }
}
