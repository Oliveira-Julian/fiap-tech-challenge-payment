using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Clientes;
using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Clientes.Interfaces;
using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Pedidos;
using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Pedidos.Interfaces;
using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Preparos;
using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Preparos.Interfaces;
using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Produtos;
using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Produtos.Interfaces;
using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Seeds;
using Microsoft.Extensions.DependencyInjection;

namespace FoodChallenge.Ioc
{
    public static class RepositoryDependency
    {
        public static IServiceCollection AddRepositoryDependency(this IServiceCollection services)
        {
            _ = services.AddScoped<IClienteSeedService, ClienteSeedService>();
            _ = services.AddScoped<IClienteRepository, ClienteRepository>();

            _ = services.AddScoped<IPedidoRepository, PedidoRepository>();
            _ = services.AddScoped<IPedidoPagamentoRepository, PedidoPagamentoRepository>();

            _ = services.AddScoped<IProdutoRepository, ProdutoRepository>();
            _ = services.AddScoped<IProdutoImagemRepository, ProdutoImagemRepository>();

            _ = services.AddScoped<IOrdemPedidoRepository, OrdemPedidoRepository>();

            return services;
        }
    }
}

