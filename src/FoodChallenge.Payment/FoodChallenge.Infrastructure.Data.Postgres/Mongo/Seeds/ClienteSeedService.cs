using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Context;
using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Clientes.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace FoodChallenge.Infrastructure.Data.Postgres.Mongo.Seeds;

public class ClienteSeedService : IClienteSeedService
{
    private readonly ILogger<ClienteSeedService> _logger;
    private readonly MongoDbContext _context;

    public ClienteSeedService(
        ILogger<ClienteSeedService> logger,
        MongoDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task SeedAsync()
    {
        await SeedClienteNaoIdentificadoAsync();
    }

    private async Task SeedClienteNaoIdentificadoAsync()
    {
        try
        {
            const string clienteNaoIdentificado = "NÃO IDENTIFICADO";

            var collection = _context.GetCollection<Repositories.Clientes.ClienteEntity>("clientes");

            var clienteExistente = await collection
                .Find(c => c.Nome != null && c.Nome.ToLower() == clienteNaoIdentificado.ToLower())
                .FirstOrDefaultAsync();

            if (clienteExistente is not null)
                return;

            var cliente = new Repositories.Clientes.ClienteEntity()
            {
                Id = Guid.Parse("a0c7ec9b-33ac-43df-85dd-4c38cadb4cda"),
                Nome = clienteNaoIdentificado,
                DataCriacao = DateTime.UtcNow,
                Ativo = true
            };

            await collection.InsertOneAsync(cliente);

            _logger.LogInformation("Cliente padrão 'NÃO IDENTIFICADO' criado com sucesso.");
        }
        catch { /* ignora caso ocorra alguma exceção */ }
    }
}
