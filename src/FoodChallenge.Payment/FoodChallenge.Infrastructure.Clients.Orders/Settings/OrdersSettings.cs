using Microsoft.Extensions.Configuration;

namespace FoodChallenge.Infrastructure.Clients.Orders.Settings;

public sealed class OrdersSettings
{
    public OrdersSettings(IConfiguration configuration)
    {
        configuration.Bind("FoodChallengeOrders", this);
    }

    public string BaseUrl { get; set; }
    public string AuthUrl { get; set; }
    public int Timeout { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public OrdersPaths Paths { get; set; }

    public class OrdersPaths
    {
        public string AtualizarPedidoPagamento { get; set; }
    }
}
