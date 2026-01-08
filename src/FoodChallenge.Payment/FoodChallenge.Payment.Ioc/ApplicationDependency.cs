using FoodChallenge.Payment.Adapter.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace FoodChallenge.Ioc;

public static class ApplicationDependency
{
    public static void AddControllersDependency(this IServiceCollection services)
    {
        services.AddScoped<PagamentoAppController>();
    }
}
