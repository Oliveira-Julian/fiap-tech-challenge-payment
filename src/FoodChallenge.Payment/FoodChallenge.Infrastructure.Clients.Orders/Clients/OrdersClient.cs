using System.Net.Http.Headers;
using FoodChallenge.Infrastructure.Clients.Orders.Constants;
using FoodChallenge.Infrastructure.Clients.Orders.Models;
using FoodChallenge.Infrastructure.Clients.Orders.Settings;
using FoodChallenge.Infrastructure.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FoodChallenge.Infrastructure.Clients.Orders.Clients;

internal sealed class OrdersClient(
        ILogger<OrdersClient> logger,
        HttpClient httpClient,
        IMemoryCache memoryCache,
        OrdersSettings ordersSettings,
        IAuthenticationClient authenticationClient) : IOrdersClient
{
    public async Task<Resposta<PedidoResponse>> AtualizarPedidoPagamentoAsync(AtualizarPedidoPagamentoRequest request, CancellationToken cancellationToken)
    {
        logger.LogDebug(Logs.InicioExecucao, ordersSettings.Paths.AtualizarPedidoPagamento);

        try
        {
            if (!await AdicionarAutorizacaoAsync(cancellationToken))
                return default;

            var response = await httpClient.PostAsJsonAsync(ordersSettings.Paths.AtualizarPedidoPagamento, request, cancellationToken);

            logger.LogDebug(Logs.FimExecucao, ordersSettings.Paths.AtualizarPedidoPagamento, response);

            return await response.MapearResponseAsync<Resposta<PedidoResponse>>(
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
                        ordersSettings.Paths.AtualizarPedidoPagamento,
                        (int)response.StatusCode,
                        mensagem
                    );
                },
                onUnauthorized: () => memoryCache.Remove(CacheAutorizacaoToken.TokenAutorizacao),
                cancellationToken: cancellationToken
            );
        }
        catch (Exception ex) when (ex is TimeoutException || ex.InnerException is TimeoutException)
        {
            logger.LogError(ex, Logs.ErroGenerico, ordersSettings.Paths.AtualizarPedidoPagamento);

            return default;
        }
    }

    private async Task<bool> AdicionarAutorizacaoAsync(CancellationToken cancellationToken)
    {
        var tokenResponse = await memoryCache.GetOrCreateAsync(CacheAutorizacaoToken.TokenAutorizacao, async (entry) =>
        {
            var tokenResponse = await authenticationClient.ObterTokenAsync(cancellationToken);

            if (tokenResponse is null)
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow;
                return default;
            }

            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(tokenResponse.ExpiresIn);

            return tokenResponse;
        });

        if (tokenResponse?.AccessToken is null)
            return false;

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenResponse.TokenType, tokenResponse.AccessToken);

        return true;
    }
}
