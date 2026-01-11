using FoodChallenge.Common.Interfaces;
using FoodChallenge.Infrastructure.Data.Mongo.Context;
using Microsoft.Extensions.DependencyInjection;

namespace FoodChallenge.Ioc
{
    public static class DatabaseDependency
    {
        public static IServiceCollection AddMongoDbDependency(this IServiceCollection services, string connectionString, string databaseName)
        {
            var context = new MongoDbContext(connectionString, databaseName);
            services.AddSingleton(context);
            services.AddScoped<IUnitOfWork>(ctx => new UnitOfWork(ctx.GetRequiredService<MongoDbContext>()));

            return services;
        }
    }
}
