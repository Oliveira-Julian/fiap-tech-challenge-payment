using Bogus;
using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Pedidos;
using FoodChallenge.Payment.UnitTests.Mocks;

namespace FoodChallenge.Payment.UnitTests.Pedidos.Entities;

public class PedidoTests : TestBase
{
    private readonly Faker _faker;

    public PedidoTests()
    {
        _faker = GetFaker();
    }

    [Fact]
    public void Cadastrar_DeveCriarNovoPedido_ComStatusRecebidoECodigoGerado()
    {
        var pedido = new Pedido();
        var idCliente = _faker.Random.Guid();
        pedido.Cadastrar(idCliente);
        Assert.Equal(PedidoStatus.Recebido, pedido.Status);
        Assert.True(pedido.Ativo);
        Assert.Equal(idCliente, pedido.IdCliente);
        Assert.NotNull(pedido.Codigo);
        Assert.Equal(6, pedido.Codigo.Length);
        Assert.True(pedido.DataCriacao <= DateTime.UtcNow);
    }

    [Fact]
    public void Cadastrar_DeveCriarPedidoSemCliente_QuandoIdClienteForNull()
    {
        var pedido = new Pedido();
        pedido.Cadastrar(null);
        Assert.Null(pedido.IdCliente);
        Assert.Equal(PedidoStatus.Recebido, pedido.Status);
    }

    [Fact]
    public void AtualizarStatusPago_DeveAtualizarStatusParaNaFila()
    {
        var pedido = PedidoMock.CriarValido();
        pedido.AtualizarStatusPago();
        Assert.Equal(PedidoStatus.NaFila, pedido.Status);
        Assert.NotNull(pedido.DataAtualizacao);
    }

    [Theory]
    [InlineData(PedidoStatus.Recebido, PedidoStatus.NaFila, true)]
    [InlineData(PedidoStatus.NaFila, PedidoStatus.EmPreparacao, true)]
    [InlineData(PedidoStatus.EmPreparacao, PedidoStatus.AguardandoRetirada, true)]
    [InlineData(PedidoStatus.AguardandoRetirada, PedidoStatus.Finalizado, true)]
    [InlineData(PedidoStatus.Recebido, PedidoStatus.EmPreparacao, false)]
    [InlineData(PedidoStatus.NaFila, PedidoStatus.Finalizado, false)]
    [InlineData(PedidoStatus.Finalizado, PedidoStatus.Recebido, false)]
    public void PermitirAlterarStatus_DeveRetornarCorreto_SegundoStatusAtualEProximo(
        PedidoStatus statusAtual, PedidoStatus proximoStatus, bool resultado)
    {
        var pedido = PedidoMock.CriarComStatus(statusAtual);
        var podeAlterar = pedido.PermitirAlterarStatus(proximoStatus);
        Assert.Equal(resultado, podeAlterar);
    }

    [Fact]
    public void ContemTodosProdutos_DeveRetornarTrue_QuandoPedidoContemTodosProdutos()
    {
        var itens = PedidoItemMock.CriarListaValida(3);
        var pedido = PedidoMock.CriarValido(itens);
        var idProdutos = itens.Select(i => i.IdProduto.Value).ToList();
        var contemTodos = pedido.ContemTodosProdutos(idProdutos);
        Assert.True(contemTodos);
    }

    [Fact]
    public void ContemTodosProdutos_DeveRetornarFalse_QuandoPedidoNaoContemTodosProdutos()
    {
        var itens = PedidoItemMock.CriarListaValida(2);
        var pedido = PedidoMock.CriarValido(itens);
        var idProdutos = itens.Select(i => i.IdProduto.Value).ToList();
        idProdutos.Add(_faker.Random.Guid());
        var contemTodos = pedido.ContemTodosProdutos(idProdutos);
        Assert.False(contemTodos);
    }

    [Fact]
    public void ObterProdutosFaltando_DeveRetornarListaVazia_QuandoPedidoContemTodosProdutos()
    {
        var itens = PedidoItemMock.CriarListaValida(3);
        var pedido = PedidoMock.CriarValido(itens);
        var idProdutos = itens.Select(i => i.IdProduto.Value).ToList();
        var produtosFaltando = pedido.ObterProdutosFaltando(idProdutos).ToList();
        Assert.Empty(produtosFaltando);
    }

    [Fact]
    public void ObterProdutosFaltando_DeveRetornarProdutosFaltantes_QuandoPedidoNaoContemTodosProdutos()
    {
        var itens = PedidoItemMock.CriarListaValida(2);
        var pedido = PedidoMock.CriarValido(itens);
        var idProdutos = itens.Select(i => i.IdProduto.Value).ToList();
        var idProdutoNovo = _faker.Random.Guid();
        idProdutos.Add(idProdutoNovo);
        var produtosFaltando = pedido.ObterProdutosFaltando(idProdutos).ToList();
        Assert.Single(produtosFaltando);
        Assert.Contains(idProdutoNovo, produtosFaltando);
    }

    [Fact]
    public void AtualizarValorTotal_DeveCalcularSomaDosProdutos()
    {
        var itens = new List<PedidoItem>
        {
            new PedidoItem { Valor = 10.5m, Quantidade = 2 },
            new PedidoItem { Valor = 15.0m, Quantidade = 3 },
            new PedidoItem { Valor = 8.0m, Quantidade = 1 }
        };
        var pedido = PedidoMock.CriarValido(itens);
        pedido.AtualizarValorTotal();
        Assert.Equal(74.0m, pedido.ValorTotal);
    }

    [Fact]
    public void PodeSerPago_DeveRetornarTrue_QuandoStatusForRecebido()
    {
        var pedido = PedidoMock.CriarComStatus(PedidoStatus.Recebido);
        var podeSerPago = pedido.PodeSerPago();
        Assert.True(podeSerPago);
    }

    [Theory]
    [InlineData(PedidoStatus.NaFila)]
    [InlineData(PedidoStatus.EmPreparacao)]
    [InlineData(PedidoStatus.AguardandoRetirada)]
    [InlineData(PedidoStatus.Finalizado)]
    public void PodeSerPago_DeveRetornarFalse_QuandoStatusNaoForRecebido(PedidoStatus status)
    {
        var pedido = PedidoMock.CriarComStatus(status);
        var podeSerPago = pedido.PodeSerPago();
        Assert.False(podeSerPago);
    }

    [Fact]
    public void ObterTodosStatusNaoFinalizados_DeveRetornarTodosStatusExcetoFinalizado()
    {
        var statuses = Pedido.ObterTodosStatusNaoFinalizados().ToList();
        Assert.DoesNotContain(PedidoStatus.Finalizado, statuses);
        Assert.Contains(PedidoStatus.Recebido, statuses);
        Assert.Contains(PedidoStatus.NaFila, statuses);
        Assert.Contains(PedidoStatus.EmPreparacao, statuses);
        Assert.Contains(PedidoStatus.AguardandoRetirada, statuses);
    }

    [Fact]
    public void AtualizarStatusPedido_DeveAtualizarStatusEDataAtualizacao()
    {
        var pedido = PedidoMock.CriarValido();
        var novoStatus = PedidoStatus.NaFila;
        pedido.AtualizarStatusPedido(novoStatus);
        Assert.Equal(novoStatus, pedido.Status);
        Assert.NotNull(pedido.DataAtualizacao);
    }

    [Fact]
    public void AtualizarItens_DeveDefinirItensNoPedido()
    {
        var pedido = PedidoMock.CriarValido();
        var novosItens = PedidoItemMock.CriarListaValida(3);
        pedido.AtualizarItens(novosItens);
        Assert.Equal(novosItens, pedido.Itens);
        Assert.Equal(3, pedido.Itens.Count());
    }

    [Fact]
    public void AtualizarValorTotal_DeveRetornarZero_QuandoItensNull()
    {
        var pedido = new Pedido { Itens = null };
        pedido.AtualizarValorTotal();
        Assert.Equal(0m, pedido.ValorTotal);
    }
}
