using Bogus;
using FoodChallenge.Payment.Domain.Clientes;

namespace FoodChallenge.Payment.UnitTests.Clientes.Entities;

public class ClienteTests : TestBase
{
    private readonly Faker _faker;

    public ClienteTests()
    {
        _faker = GetFaker();
    }

    [Fact]
    public void Construtor_DeveCriarCliente_ComCpfInformado()
    {
        var cpf = "12345678901";
        var cliente = new Cliente(cpf);
        Assert.NotEqual(Guid.Empty, cliente.Id);
        Assert.NotNull(cliente.Cpf);
        Assert.Equal(cpf, cliente.Cpf.Valor);
        Assert.True(cliente.Ativo);
    }

    [Fact]
    public void Cadastrar_DeveInicializarCliente_ComIdGerado()
    {
        var cliente = new Cliente();
        cliente.Cadastrar();
        Assert.NotEqual(Guid.Empty, cliente.Id);
        Assert.True(cliente.Ativo);
    }
}
