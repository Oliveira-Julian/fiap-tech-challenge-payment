using FoodChallenge.Common.Extensions;
using FoodChallenge.Payment.Domain.Enums;

namespace FoodChallenge.Payment.UnitTests.Domain.Enums;

public class PedidoStatusTests : TestBase
{
    [Theory]
    [InlineData(PedidoStatus.Recebido, "Recebido")]
    [InlineData(PedidoStatus.NaFila, "Na fila para preparação")]
    [InlineData(PedidoStatus.EmPreparacao, "Em preparação")]
    [InlineData(PedidoStatus.AguardandoRetirada, "Aguardando retirada")]
    [InlineData(PedidoStatus.Finalizado, "Finalizado")]
    public void GetDescription_DeveRetornarDescricaoCorreta(PedidoStatus status, string expected)
    {
        var description = status.GetDescription();
        Assert.Equal(expected, description);
    }
}
