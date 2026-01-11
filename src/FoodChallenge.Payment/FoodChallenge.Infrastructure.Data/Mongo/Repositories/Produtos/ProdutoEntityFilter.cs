using FoodChallenge.CrossCutting.Paging;

namespace FoodChallenge.Infrastructure.Data.Mongo.Repositories.Produtos;

public class ProdutoEntityFilter
{
    [FilterBy(nameof(ProdutoEntity.Categoria))]
    public IEnumerable<int> Categorias { get; set; }

    [FilterBy(nameof(ProdutoEntity.Ativo), FilterType.Equals)]
    public bool Ativo { get; set; } = true;
}

