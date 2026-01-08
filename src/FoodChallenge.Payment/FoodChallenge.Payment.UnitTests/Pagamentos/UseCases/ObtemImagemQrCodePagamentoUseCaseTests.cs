using Bogus;
using FoodChallenge.Common.Validators;
using FoodChallenge.Payment.Application.Pagamentos.UseCases;
using FoodChallenge.Payment.Application.Pedidos;
using FoodChallenge.Payment.Domain.Globalization;
using FoodChallenge.Payment.Domain.Pagamentos;
using FoodChallenge.Payment.Domain.Pedidos;
using Moq;

namespace FoodChallenge.Payment.UnitTests.Pagamentos.UseCases;

public class ObtemImagemQrCodePagamentoUseCaseTests : TestBase
{
    private readonly Faker _faker;
    private readonly ValidationContext _validationContext;
    private readonly Mock<IPedidoGateway> _pedidoGateway;
    private readonly ObtemImagemQrCodePagamentoUseCase _useCase;

    public ObtemImagemQrCodePagamentoUseCaseTests()
    {
        _faker = GetFaker();
        _validationContext = new ValidationContext();
        _pedidoGateway = new Mock<IPedidoGateway>();

        _useCase = new ObtemImagemQrCodePagamentoUseCase(
            _validationContext,
            _pedidoGateway.Object
        );
    }

    [Fact]
    public async Task ExecutarAsync_DeveRetornarNull_QuandoPedidoNaoEncontrado()
    {
        // Arrange
        var idPedido = _faker.Random.Guid();
        var mensagens = new List<string> { string.Format(Textos.NaoEncontrado, nameof(Pedido)) };

        _pedidoGateway
            .Setup(g => g.ObterPedidoComRelacionamentosAsync(idPedido, It.IsAny<CancellationToken>(), false))
            .ReturnsAsync((Pedido)null);

        // Act
        var bytes = await _useCase.ExecutarAsync(idPedido, CancellationToken.None);

        // Assert
        Assert.Null(bytes);
        Assert.True(_validationContext.HasValidations);
        Assert.Equal(mensagens, _validationContext.ValidationMessages);

        _pedidoGateway.Verify(g => g.ObterPedidoComRelacionamentosAsync(idPedido, It.IsAny<CancellationToken>(), false), Times.Once);
    }

    [Fact]
    public async Task ExecutarAsync_DeveRetornarNull_QuandoPedidoSemPagamento()
    {
        // Arrange
        var idPedido = _faker.Random.Guid();
        var pedido = new Pedido { Id = idPedido };
        var mensagens = new List<string> { string.Format(Textos.QrCodeNaoFoiGerado, pedido.Id) };

        _pedidoGateway
            .Setup(g => g.ObterPedidoComRelacionamentosAsync(idPedido, It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(pedido);

        // Act
        var bytes = await _useCase.ExecutarAsync(idPedido, CancellationToken.None);

        // Assert
        Assert.Null(bytes);
        Assert.True(_validationContext.HasValidations);
        Assert.Equal(mensagens, _validationContext.ValidationMessages);

        _pedidoGateway.Verify(g => g.ObterPedidoComRelacionamentosAsync(idPedido, It.IsAny<CancellationToken>(), false), Times.Once);
    }

    [Fact]
    public async Task ExecutarAsync_DeveRetornarImagem_QuandoPagamentoExiste()
    {
        // Arrange
        var idPedido = _faker.Random.Guid();
        var pedido = new Pedido
        {
            Id = idPedido,
            Pagamento = new Pagamento
            {
                IdPedido = idPedido,
                QrCode = "PIX|EXEMPLO|QRDATA"
            }
        };

        _pedidoGateway
            .Setup(g => g.ObterPedidoComRelacionamentosAsync(idPedido, It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(pedido);

        // Act
        var bytes = await _useCase.ExecutarAsync(idPedido, CancellationToken.None);

        // Assert
        Assert.NotNull(bytes);
        Assert.NotEmpty(bytes);
        Assert.False(_validationContext.HasValidations);
        Assert.Empty(_validationContext.ValidationMessages);

        _pedidoGateway.Verify(g => g.ObterPedidoComRelacionamentosAsync(idPedido, It.IsAny<CancellationToken>(), false), Times.Once);
    }
}
