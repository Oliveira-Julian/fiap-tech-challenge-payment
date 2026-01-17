using FoodChallenge.Infrastructure.Clients.Orders.Constants;
using FoodChallenge.Infrastructure.Clients.Orders.Models;
using FoodChallenge.Infrastructure.Clients.Orders.Settings;
using FoodChallenge.Infrastructure.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace FoodChallenge.Infrastructure.Clients.Orders.Clients;

public class AuthenticationClient(
    HttpClient httpClient,
    OrdersSettings ordersSettings,
    ILogger<AuthenticationClient> logger) : IAuthenticationClient
{
    public async Task<TokenResponse> ObterTokenAsync(CancellationToken cancellationToken = default)
    {
        logger.LogDebug(Logs.InicioExecucao, ordersSettings.AuthUrl);

        try
        {
            var response = await httpClient.SendAsync(CriarHttpRequestMessage(), cancellationToken);

            logger.LogDebug(Logs.FimExecucao, ordersSettings.AuthUrl, response);

            return await response.MapearResponseAsync<TokenResponse>(
                addValidationAsync: (codigo, mensagem) =>
                {
                    logger.LogWarning(
                        Logs.ErroResponse,
                        ordersSettings.AuthUrl,
                        (int)response.StatusCode,
                        mensagem
                    );
                    return Task.CompletedTask;
                },
                error: mensagem =>
                {
                    logger.LogError(
                        Logs.ErroResponse,
                        ordersSettings.AuthUrl,
                        (int)response.StatusCode,
                        mensagem
                    );
                },
                cancellationToken: cancellationToken
            );
        }
        catch (Exception ex) when (ex is TimeoutException || ex.InnerException is TimeoutException)
        {
            logger.LogError(ex, Logs.ErroGenerico, ordersSettings.AuthUrl);

            return default;
        }
    }

    private HttpRequestMessage CriarHttpRequestMessage()
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", ordersSettings.ClientId },
            { "client_secret", ordersSettings.ClientSecret },
            { "scope", ordersSettings.Scope }
        });

        return new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(ordersSettings.AuthUrl),
            Content = content
        };
    }
}
