using FoodChallenge.Payment.Adapter.Controllers;
using FoodChallenge.Payment.Application.Pagamentos.Models.Requests;
using FoodChallenge.Payment.Application.Pagamentos.Models.Responses;
using FoodChallenge.Payment.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace FoodChallenge.Payment.Api.Controllers;

[ApiController]
[Route("[Controller]")]
public class PagamentoController(
    ILogger<PagamentoController> logger,
    PagamentoAppController pagamentoAppController) : ControllerBase
{
    /// <summary>
    /// Gera imagem do QRCode.
    /// </summary>
    /// <param name="id">Informações da requição.</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    [HttpGet("pedido/{idPedido}/qrcode")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ObterQrCodeAsync(Guid idPedido, CancellationToken cancellationToken)
    {
        logger.LogInformation(Logs.InicioExecucaoServico, nameof(PagamentoController), nameof(ObterQrCodeAsync));

        var resposta = await pagamentoAppController.ObterImagemQrCodeAsync(idPedido, cancellationToken);

        logger.LogDebug(Logs.FimExecucaoServico, nameof(PagamentoController), nameof(ObterQrCodeAsync), null);

        return resposta is null ? NoContent() : File(resposta, "image/png");
    }

    /// <summary>
    /// Cria um novo pagamento via MercadoPago.
    /// </summary>
    /// <param name="request">Dados do pagamento.</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Retorna os dados do pagamento criado incluindo o IdPagamento</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PagamentoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CriarPagamentoAsync([FromBody] CriarPagamentoRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation(Logs.InicioExecucaoServico, nameof(PagamentoController), nameof(CriarPagamentoAsync));

        var resposta = await pagamentoAppController.CriarPagamentoAsync(request, cancellationToken);

        logger.LogDebug(Logs.FimExecucaoServico, nameof(PagamentoController), nameof(CriarPagamentoAsync), resposta);

        return Ok(resposta);
    }
}
