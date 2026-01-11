using Bogus;
using FoodChallenge.Common.Validators;
using FoodChallenge.Payment.Application.Pagamentos;
using FoodChallenge.Payment.Application.Pagamentos.UseCases;
using FoodChallenge.Payment.Domain.Globalization;
using FoodChallenge.Payment.Domain.Pagamentos;
using Moq;

namespace FoodChallenge.Payment.UnitTests.Pagamentos.UseCases;

public class ObtemImagemQrCodePagamentoUseCaseTests : TestBase
{
    private readonly Faker _faker;
    private readonly ValidationContext _validationContext;
    private readonly Mock<IPagamentoGateway> _pagamentoGateway;
    private readonly ObtemImagemQrCodePagamentoUseCase _useCase;

    public ObtemImagemQrCodePagamentoUseCaseTests()
    {
        _faker = GetFaker();
        _validationContext = new ValidationContext();
        _pagamentoGateway = new Mock<IPagamentoGateway>();

        _useCase = new ObtemImagemQrCodePagamentoUseCase(
            _validationContext,
            _pagamentoGateway.Object
        );
    }

    [Fact]
    public async Task ExecutarAsync_DeveRetornarNull_QuandoPagamentoNaoEncontrado()
    {
        var idPedido = _faker.Random.Guid();
        var mensagens = new List<string> { string.Format(Textos.NaoEncontrado, nameof(Pagamento)) };

        _pagamentoGateway
            .Setup(g => g.ObterPagamentoPorIdPedidoAsync(idPedido, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Pagamento)null);
        var bytes = await _useCase.ExecutarAsync(idPedido, CancellationToken.None);
        Assert.Null(bytes);
        Assert.True(_validationContext.HasValidations);
        Assert.Equal(mensagens, _validationContext.ValidationMessages);

        _pagamentoGateway.Verify(g => g.ObterPagamentoPorIdPedidoAsync(idPedido, It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task ExecutarAsync_DeveRetornarNull_QuandoPagamentoSemQrCode()
    {
        var idPedido = _faker.Random.Guid();
        var pagamento = new Pagamento { IdPedido = idPedido, QrCode = null };
        var mensagens = new List<string> { string.Format(Textos.QrCodeNaoFoiGerado, idPedido) };

        _pagamentoGateway
            .Setup(g => g.ObterPagamentoPorIdPedidoAsync(idPedido, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagamento);
        var bytes = await _useCase.ExecutarAsync(idPedido, CancellationToken.None);
        Assert.Null(bytes);
        Assert.True(_validationContext.HasValidations);
        Assert.Equal(mensagens, _validationContext.ValidationMessages);

        _pagamentoGateway.Verify(g => g.ObterPagamentoPorIdPedidoAsync(idPedido, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecutarAsync_DeveRetornarImagem_QuandoPagamentoExiste()
    {
        var idPedido = _faker.Random.Guid();
        var pagamento = new Pagamento
        {
            IdPedido = idPedido,
            QrCode = "PIX|EXEMPLO|QRDATA"
        };

        _pagamentoGateway
            .Setup(g => g.ObterPagamentoPorIdPedidoAsync(idPedido, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagamento);
        var bytes = await _useCase.ExecutarAsync(idPedido, CancellationToken.None);
        Assert.NotNull(bytes);
        Assert.NotEmpty(bytes);
        Assert.False(_validationContext.HasValidations);
        Assert.Empty(_validationContext.ValidationMessages);

        _pagamentoGateway.Verify(g => g.ObterPagamentoPorIdPedidoAsync(idPedido, It.IsAny<CancellationToken>()), Times.Once);
    }
}
