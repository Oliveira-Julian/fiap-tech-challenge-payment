using Bogus;
using FoodChallenge.Payment.Domain.Clientes;

namespace FoodChallenge.Payment.UnitTests.Mocks;

public static class ClienteMock
{
    public static Faker<Cliente> GetFaker()
    {
        return new Faker<Cliente>()
            .CustomInstantiator(faker => new Cliente
            {
                Id = Guid.NewGuid(),
                Nome = faker.Person.FullName,
                Ativo = true,
                DataCriacao = DateTime.UtcNow
            });
    }

    public static Cliente CriarValido() => GetFaker().Generate();

    public static List<Cliente> CriarListaValida(int quantidade) => GetFaker().Generate(quantidade);
}
