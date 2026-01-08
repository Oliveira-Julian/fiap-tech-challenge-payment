using FoodChallenge.Payment.Domain.Clientes.ValueObjects;
using FoodChallenge.Payment.Domain.Entities;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Domain.Clientes;

public sealed class Cliente : DomainBase
{
    public Cpf Cpf { get; set; }
    public string Nome { get; set; }
    public Email Email { get; set; }
    public IEnumerable<Pedido> Pedidos { get; set; }

    public Cliente()
    {
    }

    public Cliente(string cpf)
    {
        Cpf = new Cpf(cpf);
        Id = Guid.NewGuid();
        Cadastrar();
    }

    public void Cadastrar()
    {
        Id = Guid.NewGuid();
        Ativo = true;
    }
}
