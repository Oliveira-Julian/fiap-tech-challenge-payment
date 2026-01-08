using FoodChallenge.Payment.Domain.Clientes.ValueObjects;

namespace FoodChallenge.Payment.UnitTests.Clientes.ValueObjects;

public class EmailTests : TestBase
{
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user@domain.com.br")]
    [InlineData("name.surname@company.co")]
    [InlineData("valid_email@test.org")]
    public void EhValido_DeveRetornarTrue_QuandoEmailValido(string emailValido)
    {
        var email = new Email(emailValido);
        var ehValido = email.EhValido();
        Assert.True(ehValido);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("invalid")]
    [InlineData("@example.com")]
    [InlineData("test@")]
    [InlineData("test")]
    [InlineData("test@com")]
    public void EhValido_DeveRetornarFalse_QuandoEmailInvalido(string emailInvalido)
    {
        var email = new Email(emailInvalido);
        var ehValido = email.EhValido();
        Assert.False(ehValido);
    }

    [Fact]
    public void Construtor_DeveTrimEConverterParaLowerCase()
    {
        var emailOriginal = "  TEST@EXAMPLE.COM  ";
        var email = new Email(emailOriginal);
        Assert.Equal("test@example.com", email.Valor);
    }

    [Fact]
    public void Construtor_DeveRetornarStringVazia_QuandoNull()
    {
        var email = new Email(null);
        Assert.Equal(string.Empty, email.Valor);
    }

    [Fact]
    public void Construtor_DeveRetornarStringVazia_QuandoVazio()
    {
        var email = new Email("");
        Assert.Equal(string.Empty, email.Valor);
    }

    [Fact]
    public void Construtor_DeveRetornarStringVazia_QuandoSomenteEspacos()
    {
        var email = new Email("   ");
        Assert.Equal(string.Empty, email.Valor);
    }
}
