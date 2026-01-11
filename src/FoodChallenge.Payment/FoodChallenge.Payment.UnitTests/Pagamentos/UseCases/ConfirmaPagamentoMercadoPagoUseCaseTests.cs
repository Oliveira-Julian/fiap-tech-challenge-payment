using Bogus;
using FoodChallenge.Common.Extensions;
using FoodChallenge.Common.Interfaces;
using FoodChallenge.Common.Validators;
using FoodChallenge.Payment.Application.Pagamentos;
using FoodChallenge.Payment.Application.Pagamentos.UseCases;
using FoodChallenge.Payment.Application.Pedidos;
using FoodChallenge.Payment.Application.Preparos;
using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Globalization;
using FoodChallenge.Payment.Domain.Pagamentos;
using FoodChallenge.Payment.Domain.Pedidos;
using FoodChallenge.Payment.Domain.Preparos;
using Moq;

namespace FoodChallenge.Payment.UnitTests.Pagamentos.UseCases;

public class ConfirmaPagamentoMercadoPagoUseCaseTests : TestBase
{
    private readonly Faker _faker;
    private readonly ValidationContext _validationContext;
    private readonly Mock<IPagamentoGateway> _pagamentoGateway;
    private readonly Mock<IPedidoGateway> _pedidoGateway;
    private readonly Mock<IOrdemPedidoGateway> _ordemPedidoGateway;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly ConfirmaPagamentoMercadoPagoUseCase _useCase;

    public ConfirmaPagamentoMercadoPagoUseCaseTests()
    {
        _faker = new Faker();
        _faker = GetFaker();
        _validationContext = new ValidationContext();
        _pagamentoGateway = new Mock<IPagamentoGateway>();
        _pedidoGateway = new Mock<IPedidoGateway>();
        _ordemPedidoGateway = new Mock<IOrdemPedidoGateway>();
        _unitOfWork = new Mock<IUnitOfWork>();

        _useCase = new ConfirmaPagamentoMercadoPagoUseCase(
            _validationContext,
            _unitOfWork.Object,
            _pagamentoGateway.Object,
            _pedidoGateway.Object,
            _ordemPedidoGateway.Object
        );
    }

    [Fact]
    public async Task ExecutarAsync_DeveRetornarValidacao_QuandoWebhookInvalido()
    {
        var notificacao = new NotificacaoMercadoPago { Tipo = "invalid" };
        var result = await _useCase.ExecutarAsync(notificacao, CancellationToken.None);
        Assert.Null(result);
        Assert.Contains(Textos.WebhookAcaoOuTipoNaoPermitido, _validationContext.ValidationMessages);
    }

    [Fact]
    public async Task ExecutarAsync_DeveRetornarValidacao_QuandoPagamentoNaoEncontrado()
    {
        var notificacao = new NotificacaoMercadoPago { Tipo = "payment", Acao = "payment.created" };
        var validationMessages = new List<string>
        {
            string.Format(Textos.NaoEncontrado, nameof(Pedido))
        };

        _pagamentoGateway
            .Setup(p => p.ObterPagamentoIdMercadoPagoAsync(notificacao.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Pagamento)null);
        var result = await _useCase.ExecutarAsync(notificacao, CancellationToken.None);
        Assert.Null(result);
        Assert.True(_validationContext.HasValidations);
        Assert.Equal(validationMessages, _validationContext.ValidationMessages);

        _pagamentoGateway.Verify(g => g.ObterPagamentoIdMercadoPagoAsync(notificacao.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecutarAsync_DeveRetornarValidacao_QuandoPedidoNaoEncontrado()
    {
        var validationMessages = new List<string>
        {
            string.Format(Textos.NaoEncontrado, nameof(Pedido))
        };
        var notificacao = new NotificacaoMercadoPago { Tipo = "payment", Acao = "payment.created" };

        var pagamento = new Pagamento
        {
            IdPedido = Guid.NewGuid(),
            IdMercadoPagoOrdem = _faker.Random.Guid().ToString()
        };

        _pagamentoGateway
            .Setup(p => p.ObterPagamentoIdMercadoPagoAsync(notificacao.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagamento);

        _pagamentoGateway
            .Setup(p => p.ObterPedidoMercadoPagoAsync(pagamento.IdMercadoPagoOrdem, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Pagamento { IdMercadoPagoPagamento = _faker.Random.Guid().ToString() });

        _pedidoGateway
            .Setup(p => p.ObterPedidoComRelacionamentosAsync(pagamento.IdPedido.Value, It.IsAny<CancellationToken>(), false))
            .ReturnsAsync((Pedido)null);
        var result = await _useCase.ExecutarAsync(notificacao, CancellationToken.None);
        Assert.Null(result);
        Assert.True(_validationContext.HasValidations);
        Assert.Equal(validationMessages, _validationContext.ValidationMessages);

        _pagamentoGateway.Verify(g => g.ObterPagamentoIdMercadoPagoAsync(notificacao.Id, It.IsAny<CancellationToken>()), Times.Once);
        _pagamentoGateway.Verify(g => g.ObterPedidoMercadoPagoAsync(pagamento.IdMercadoPagoOrdem, It.IsAny<CancellationToken>()), Times.Once);
        _pedidoGateway.Verify(g => g.ObterPedidoComRelacionamentosAsync(pagamento.IdPedido.Value, It.IsAny<CancellationToken>(), false), Times.Once);
    }

    [Fact]
    public async Task ExecutarAsync_DeveAtualizarSomentePagamento_QuandoNaoAprovado()
    {
        _unitOfWork.Setup(u => u.BeginTransaction());
        _unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

        var notificacao = new NotificacaoMercadoPago { Tipo = "payment", Acao = "payment.canceled" };

        var pagamento = new Pagamento
        {
            IdPedido = Guid.NewGuid(),
            IdMercadoPagoOrdem = _faker.Random.Guid().ToString(),
            Status = notificacao.Status
        };

        var pedido = new Pedido { Id = pagamento.IdPedido, Pagamento = pagamento };

        _pagamentoGateway
            .Setup(p => p.ObterPagamentoIdMercadoPagoAsync(notificacao.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagamento);

        _pagamentoGateway
            .Setup(p => p.ObterPedidoMercadoPagoAsync(pagamento.IdMercadoPagoOrdem, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Pagamento { IdMercadoPagoPagamento = _faker.Random.Guid().ToString() });

        _pedidoGateway
            .Setup(p => p.ObterPedidoComRelacionamentosAsync(pagamento.IdPedido.Value, It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(pedido);
        var result = await _useCase.ExecutarAsync(notificacao, CancellationToken.None);
        Assert.Equal(pedido, result);
        Assert.Equal(PagamentoStatus.Recusado, result.Pagamento.Status);
        _pagamentoGateway.Verify(p => p.AtualizarPagamento(pagamento), Times.Once);
        _pedidoGateway.Verify(p => p.AtualizarPedido(It.IsAny<Pedido>()), Times.Never);
        _ordemPedidoGateway.Verify(o => o.CadastrarOrdemPedidoAsync(It.IsAny<OrdemPedido>(), It.IsAny<CancellationToken>()), Times.Never);
        _pedidoGateway.Verify(g => g.ObterPedidoComRelacionamentosAsync(pagamento.IdPedido.Value, It.IsAny<CancellationToken>(), false), Times.Once);
    }

    [Fact]
    public async Task ExecutarAsync_DeveAtualizarPedidoEOrdem_QuandoPagamentoAprovado()
    {
        _unitOfWork.Setup(u => u.BeginTransaction());
        _unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

        var notificacao = new NotificacaoMercadoPago { Tipo = "payment", Acao = "payment.processed" };

        var pagamento = new Pagamento
        {
            IdPedido = Guid.NewGuid(),
            IdMercadoPagoOrdem = _faker.Random.Guid().ToString(),
            Status = notificacao.Status
        };

        var pedido = new Pedido { Id = pagamento.IdPedido, Pagamento = pagamento };

        _pagamentoGateway
            .Setup(p => p.ObterPagamentoIdMercadoPagoAsync(notificacao.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagamento);

        _pagamentoGateway
            .Setup(p => p.ObterPedidoMercadoPagoAsync(pagamento.IdMercadoPagoOrdem, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Pagamento { IdMercadoPagoPagamento = _faker.Random.Guid().ToString() });

        _pedidoGateway
            .Setup(p => p.ObterPedidoComRelacionamentosAsync(pagamento.IdPedido.Value, It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(pedido);
        var result = await _useCase.ExecutarAsync(notificacao, CancellationToken.None);
        Assert.Equal(pedido, result);
        Assert.Equal(PagamentoStatus.Aprovado, result.Pagamento.Status);
        _pagamentoGateway.Verify(p => p.AtualizarPagamento(pagamento), Times.Once);
        _pedidoGateway.Verify(p => p.AtualizarPedido(pedido), Times.Once);
        _ordemPedidoGateway.Verify(o => o.CadastrarOrdemPedidoAsync(It.IsAny<OrdemPedido>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecutarAsync_DeveRetornarNull_QuandoPedidoNaoPodeSerPago()
    {
        _unitOfWork.Setup(u => u.BeginTransaction());
        _unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

        var notificacao = new NotificacaoMercadoPago { Tipo = "payment", Acao = "payment.processed" };

        var pagamento = new Pagamento
        {
            IdPedido = Guid.NewGuid(),
            IdMercadoPagoOrdem = _faker.Random.Guid().ToString(),
            Status = notificacao.Status
        };

        var pedido = new Pedido { Id = pagamento.IdPedido, Pagamento = pagamento };
        pedido.AtualizarStatusPago();
        var validationMessages = new List<string>
        {
            string.Format(Textos.PagamentoStatusNaoPermitido, pedido.Status.GetDescription())
        };

        _pagamentoGateway
            .Setup(p => p.ObterPagamentoIdMercadoPagoAsync(notificacao.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagamento);

        _pagamentoGateway
            .Setup(p => p.ObterPedidoMercadoPagoAsync(pagamento.IdMercadoPagoOrdem, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Pagamento { IdMercadoPagoPagamento = _faker.Random.Guid().ToString() });

        _pedidoGateway
            .Setup(p => p.ObterPedidoComRelacionamentosAsync(pagamento.IdPedido.Value, It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(pedido);
        var result = await _useCase.ExecutarAsync(notificacao, CancellationToken.None);
        Assert.Null(result);
        Assert.True(_validationContext.HasValidations);
        Assert.Equal(validationMessages, _validationContext.ValidationMessages);

        _pagamentoGateway.Verify(p => p.AtualizarPagamento(It.IsAny<Pagamento>()), Times.Never);
        _pedidoGateway.Verify(p => p.AtualizarPedido(It.IsAny<Pedido>()), Times.Never);
        _ordemPedidoGateway.Verify(o => o.CadastrarOrdemPedidoAsync(It.IsAny<OrdemPedido>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(u => u.BeginTransaction(), Times.Never);
    }

    [Fact]
    public async Task ExecutarAsync_DeveRetornarValidacao_QuandoPagamentoMercadoPagoNaoEncontrado()
    {
        var notificacao = new NotificacaoMercadoPago { Tipo = "payment", Acao = "payment.created" };
        var mensagens = new List<string> { string.Format(Textos.NaoEncontrado, nameof(Pedido)) };

        var pagamento = new Pagamento
        {
            IdPedido = Guid.NewGuid(),
            IdMercadoPagoOrdem = _faker.Random.Guid().ToString()
        };

        _pagamentoGateway
            .Setup(p => p.ObterPagamentoIdMercadoPagoAsync(notificacao.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagamento);

        _pagamentoGateway
            .Setup(p => p.ObterPedidoMercadoPagoAsync(pagamento.IdMercadoPagoOrdem, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Pagamento)null);
        var result = await _useCase.ExecutarAsync(notificacao, CancellationToken.None);
        Assert.Null(result);
        Assert.True(_validationContext.HasValidations);
        Assert.Equal(mensagens, _validationContext.ValidationMessages);

        _pagamentoGateway.Verify(p => p.ObterPagamentoIdMercadoPagoAsync(notificacao.Id, It.IsAny<CancellationToken>()), Times.Once);
        _pagamentoGateway.Verify(p => p.ObterPedidoMercadoPagoAsync(pagamento.IdMercadoPagoOrdem, It.IsAny<CancellationToken>()), Times.Once);

        _pedidoGateway.Verify(p => p.ObterPedidoComRelacionamentosAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Never);
        _unitOfWork.Verify(u => u.BeginTransaction(), Times.Never);
        _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        _unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
    }

    [Fact]
    public async Task ExecutarAsync_DeveRetornarValidacao_QuandoPagamentoMercadoPagoSemIdPagamento()
    {
        var notificacao = new NotificacaoMercadoPago { Tipo = "payment", Acao = "payment.created" };
        var mensagens = new List<string> { string.Format(Textos.NaoEncontrado, nameof(Pedido)) };

        var pagamento = new Pagamento
        {
            IdPedido = Guid.NewGuid(),
            IdMercadoPagoOrdem = _faker.Random.Guid().ToString()
        };

        _pagamentoGateway
            .Setup(p => p.ObterPagamentoIdMercadoPagoAsync(notificacao.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagamento);

        _pagamentoGateway
            .Setup(p => p.ObterPedidoMercadoPagoAsync(pagamento.IdMercadoPagoOrdem, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Pagamento { IdMercadoPagoPagamento = string.Empty });
        var result = await _useCase.ExecutarAsync(notificacao, CancellationToken.None);
        Assert.Null(result);
        Assert.True(_validationContext.HasValidations);
        Assert.Equal(mensagens, _validationContext.ValidationMessages);

        _pagamentoGateway.Verify(p => p.ObterPagamentoIdMercadoPagoAsync(notificacao.Id, It.IsAny<CancellationToken>()), Times.Once);
        _pagamentoGateway.Verify(p => p.ObterPedidoMercadoPagoAsync(pagamento.IdMercadoPagoOrdem, It.IsAny<CancellationToken>()), Times.Once);

        _pedidoGateway.Verify(p => p.ObterPedidoComRelacionamentosAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Never);
        _unitOfWork.Verify(u => u.BeginTransaction(), Times.Never);
        _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        _unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
    }

    [Fact]
    public async Task ExecutarAsync_EmCasoDeExcecao_DeveFazerRollback_ERethrow()
    {
        _unitOfWork.Setup(u => u.BeginTransaction());
        _unitOfWork.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

        var notificacao = new NotificacaoMercadoPago { Tipo = "payment", Acao = "payment.processed" };

        _pagamentoGateway
            .Setup(p => p.ObterPagamentoIdMercadoPagoAsync(notificacao.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Erro inesperado"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecutarAsync(notificacao, CancellationToken.None));

        _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
    }
}
