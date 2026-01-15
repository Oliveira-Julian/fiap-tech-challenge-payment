using Bogus;
using FoodChallenge.Common.Interfaces;
using FoodChallenge.Common.Validators;
using FoodChallenge.Payment.Application.Pagamentos;
using FoodChallenge.Payment.Application.Pagamentos.UseCases;
using FoodChallenge.Payment.Application.Pedidos;
using FoodChallenge.Payment.Domain.Globalization;
using FoodChallenge.Payment.Domain.Pagamentos;
using FoodChallenge.Payment.Domain.Pedidos;
using Moq;

namespace FoodChallenge.Payment.UnitTests.Pagamentos.UseCases;

public class GeraQrCodePagamentoUseCaseTests : TestBase
{
    private readonly Faker _faker;
    private readonly ValidationContext _validationContext;
    private readonly Mock<IPagamentoGateway> _pagamentoGateway;
    private readonly Mock<IPedidoGateway> _pedidoGateway;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly GeraQrCodePagamentoUseCase _useCase;

    public GeraQrCodePagamentoUseCaseTests()
    {
        _faker = GetFaker();
        _validationContext = new ValidationContext();
        _pagamentoGateway = new Mock<IPagamentoGateway>();
        _pedidoGateway = new Mock<IPedidoGateway>();
        _unitOfWork = new Mock<IUnitOfWork>();

        _useCase = new GeraQrCodePagamentoUseCase(
            _validationContext,
            _unitOfWork.Object,
            _pedidoGateway.Object,
            _pagamentoGateway.Object
        );
    }

    [Fact]
    public async Task ExecutarAsync_DeveRetornarNull_QuandoPedidoNaoEncontrado()
    {
        var idPedido = _faker.Random.Guid();
        var validationMessages = new List<string> { string.Format(Textos.NaoEncontrado, nameof(Pedido)) };

        _pedidoGateway
            .Setup(p => p.ObterPedidoAsync(idPedido, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Pedido)null);
        var result = await _useCase.ExecutarAsync(idPedido, CancellationToken.None);
        Assert.Null(result);
        Assert.True(_validationContext.HasValidations);
        Assert.Equal(validationMessages, _validationContext.ValidationMessages);

        _pedidoGateway.Verify(p => p.ObterPedidoAsync(idPedido, It.IsAny<CancellationToken>()), Times.Once);

        _pagamentoGateway.Verify(p => p.CadastrarPedidoMercadoPagoAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()), Times.Never);
        _pagamentoGateway.Verify(p => p.AdicionarPagamentoAsync(It.IsAny<Pagamento>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(u => u.BeginTransaction(), Times.Never);
        _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task ExecutarAsync_DeveCadastrarPagamento_E_RecarregarPedido_QuandoSucesso()
    {
        var idPedido = _faker.Random.Guid();
        var pedidoInicial = new Pedido { Id = idPedido };
        var pagamentoGerado = new Pagamento { IdPedido = idPedido };
        var pedidoRecarregado = new Pedido { Id = idPedido };

        _pedidoGateway
            .Setup(p => p.ObterPedidoAsync(idPedido, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedidoInicial);

        _pagamentoGateway
            .Setup(p => p.CadastrarPedidoMercadoPagoAsync(pedidoInicial, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagamentoGerado);

        _unitOfWork.Setup(u => u.BeginTransaction());
        _pagamentoGateway
            .Setup(p => p.AdicionarPagamentoAsync(pagamentoGerado, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWork
            .Setup(u => u.CommitAsync())
            .Returns(Task.CompletedTask);

        _pedidoGateway
            .SetupSequence(p => p.ObterPedidoAsync(idPedido, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedidoInicial)
            .ReturnsAsync(pedidoRecarregado);
        var result = await _useCase.ExecutarAsync(idPedido, CancellationToken.None);
        Assert.Same(pedidoRecarregado, result);

        _pedidoGateway.Verify(p => p.ObterPedidoAsync(idPedido, It.IsAny<CancellationToken>()), Times.Exactly(2));
        _pagamentoGateway.Verify(p => p.CadastrarPedidoMercadoPagoAsync(pedidoInicial, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.BeginTransaction(), Times.Once);
        _pagamentoGateway.Verify(p => p.AdicionarPagamentoAsync(pagamentoGerado, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecutarAsync_EmCasoDeExcecao_DeveChamarCommit_ERethrow()
    {
        var idPedido = _faker.Random.Guid();
        var pedido = new Pedido { Id = idPedido };

        _pedidoGateway
            .Setup(p => p.ObterPedidoAsync(idPedido, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedido);

        _pagamentoGateway
            .Setup(p => p.CadastrarPedidoMercadoPagoAsync(pedido, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Pagamento { IdPedido = idPedido });

        _unitOfWork.Setup(u => u.BeginTransaction());
        var esperado = new InvalidOperationException("Falha ao adicionar pagamento");
        _pagamentoGateway
            .Setup(p => p.AdicionarPagamentoAsync(It.IsAny<Pagamento>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(esperado);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecutarAsync(idPedido, CancellationToken.None));
        Assert.Same(esperado, ex);

        _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }
}
