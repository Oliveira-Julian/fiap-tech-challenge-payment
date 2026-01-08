using FoodChallenge.Payment.Domain.Entities;
using FoodChallenge.Payment.Domain.Enums;

namespace FoodChallenge.Payment.Domain.Produtos;

public sealed class Produto : DomainBase
{
    public ProdutoCategoria Categoria { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
}
