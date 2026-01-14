using FoodChallenge.Infrastructure.Clients.Orders.Models;

namespace FoodChallenge.Infrastructure.Clients.Orders.Clients;

public interface IAuthenticationClient
{
    Task<TokenResponse> ObterTokenAsync(CancellationToken cancellationToken = default);
}
