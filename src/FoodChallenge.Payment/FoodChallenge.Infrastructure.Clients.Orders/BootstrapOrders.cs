using FoodChallenge.Infrastructure.Clients.Orders.Clients;
using FoodChallenge.Infrastructure.Clients.Orders.Settings;
using FoodChallenge.Infrastructure.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodChallenge.Infrastructure.Clients.Orders;

public static class BootstrapOrders
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        var settings = new OrdersSettings(configuration);

        services.AddHttpClient<IOrdersClient, OrdersClient>(client =>
        {
            client.ConfigureBaseAddressForRestApi(settings.BaseUrl);
            client.ConfigureTimeout(settings.Timeout);
        })
          .AddHeaderPropagation();

        services.AddHttpClient<IAuthenticationClient, AuthenticationClient>(client =>
        {
            client.ConfigureTimeout(settings.Timeout);
        })
          .AddHeaderPropagation();

        services.AddMemoryCache();

        services.AddSingleton<OrdersSettings>();
    }
}
