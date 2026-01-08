using FoodChallenge.Infrastructure.Data.Postgres.EntityFramework.Repositories.Clientes;
using FoodChallenge.Payment.Adapter.Mappers;
using FoodChallenge.Payment.Domain.Clientes;
using FoodChallenge.Payment.Domain.Clientes.ValueObjects;

namespace FoodChallenge.Payment.Adapter.Presenters;

public static class ClienteMapper
{
    public static Cliente ToDomain(ClienteEntity clienteEntity)
    {
        if (clienteEntity is null) return default;

        return new Cliente()
        {
            Id = clienteEntity.Id,
            Ativo = clienteEntity.Ativo,
            DataAtualizacao = clienteEntity.DataAtualizacao,
            DataCriacao = clienteEntity.DataCriacao,
            DataExclusao = clienteEntity.DataExclusao,
            Cpf = new Cpf(clienteEntity.Cpf),
            Email = new Email(clienteEntity.Email),
            Nome = clienteEntity.Nome,
            Pedidos = clienteEntity.Pedidos?.Select(PedidoMapper.ToDomain)
        };
    }

    public static ClienteEntity ToEntity(Cliente cliente)
    {
        if (cliente is null) return default;

        return new ClienteEntity()
        {
            Id = cliente.Id,
            Ativo = cliente.Ativo,
            DataAtualizacao = cliente.DataAtualizacao,
            DataCriacao = cliente.DataCriacao,
            DataExclusao = cliente.DataExclusao,
            Cpf = cliente.Cpf?.ToString(),
            Email = cliente.Email?.Valor,
            Nome = cliente.Nome,
            Pedidos = cliente.Pedidos?.Select(PedidoMapper.ToEntity)?.ToList()
        };
    }
}
