using FoodChallenge.Common.Entities;
using FoodChallenge.Payment.Adapter.Controllers;
using FoodChallenge.Payment.Application.Pagamentos.Models.Requests;
using FoodChallenge.Payment.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodChallenge.Payment.Api.Controllers;

[ApiController]
[Route("[Controller]")]
[Authorize(Policy = AuthorizationPolicies.PaymentsApi)]
public class WebhooksController(
    PagamentoAppController pagamentoAppController,
    ILogger<WebhooksController> logger) : ControllerBase
{
    /// <summary>
    /// Relizar pagamento do Pedido.
    /// </summary>
    /// <param name="request">Informa��es do webhook disparado pelo Mercado Pago.</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    [HttpPost("mercadoPago/pagamento")]
    [ProducesResponseType(typeof(Resposta), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Resposta), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Resposta), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(Resposta), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resposta>> ConfirmarPagamntoAsync([FromBody] WebhookMercadoPagoPagamentoRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation(Logs.InicioExecucaoServico, nameof(WebhooksController), nameof(ConfirmarPagamntoAsync));

        var resposta = await pagamentoAppController.ConfirmarPagamentoMercadoPagoAsync(request, cancellationToken);

        logger.LogDebug(Logs.FimExecucaoServico, nameof(WebhooksController), nameof(ConfirmarPagamntoAsync), resposta);

        return resposta is null ? NoContent() : Ok(resposta);
    }
}
