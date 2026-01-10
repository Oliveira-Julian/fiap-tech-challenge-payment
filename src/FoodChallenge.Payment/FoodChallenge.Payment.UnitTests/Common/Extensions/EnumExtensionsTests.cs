using FoodChallenge.Common.Extensions;
using System.ComponentModel;

namespace FoodChallenge.Payment.UnitTests.Common.Extensions;

public class EnumExtensionsTests : TestBase
{
    public enum TestEnumWithDescription
    {
        [Description("Valor Um")]
        ValueOne,

        [Description("Valor Dois")]
        ValueTwo,

        ValueThree
    }

    [Theory]
    [InlineData(TestEnumWithDescription.ValueOne, "Valor Um")]
    [InlineData(TestEnumWithDescription.ValueTwo, "Valor Dois")]
    public void GetDescription_DeveRetornarDescricao_QuandoExisteAttribute(TestEnumWithDescription value, string expected)
    {
        var description = value.GetDescription();
        Assert.Equal(expected, description);
    }

    [Fact]
    public void GetDescription_DeveRetornarNomeEnum_QuandoNaoExisteAttribute()
    {
        var value = TestEnumWithDescription.ValueThree;
        var description = value.GetDescription();
        Assert.Equal("ValueThree", description);
    }

    [Theory]
    [InlineData(TestEnumWithDescription.ValueOne, "Valor Um")]
    [InlineData(TestEnumWithDescription.ValueTwo, "Valor Dois")]
    public void GetCode_DeveRetornarCodigo_QuandoExisteAttribute(TestEnumWithDescription value, string expected)
    {
        var code = value.GetCode();
        Assert.Equal(expected, code);
    }

    [Fact]
    public void GetCode_DeveRetornarNomeEnum_QuandoNaoExisteAttribute()
    {
        var value = TestEnumWithDescription.ValueThree;
        var code = value.GetCode();
        Assert.Equal("ValueThree", code);
    }
}
