using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Produtos;
using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Produtos;

namespace FoodChallenge.Payment.Adapter.Mappers;

public static class ProdutoMapper
{
    public static Produto ToDomain(ProdutoEntity produtoEntity)
    {
        if (produtoEntity is null) return default;

        return new Produto
        {
            Id = produtoEntity.Id,
            DataAtualizacao = produtoEntity.DataAtualizacao,
            DataCriacao = produtoEntity.DataCriacao,
            DataExclusao = produtoEntity.DataExclusao,
            Ativo = produtoEntity.Ativo,
            Categoria = (ProdutoCategoria)produtoEntity.Categoria,
            Nome = produtoEntity.Nome,
            Descricao = produtoEntity.Descricao,
            Preco = produtoEntity.Preco
        };
    }

    public static ProdutoEntity ToEntity(Produto produto)
    {
        if (produto is null) return default;

        return new ProdutoEntity
        {
            Id = produto.Id,
            DataAtualizacao = produto.DataAtualizacao,
            DataCriacao = produto.DataCriacao,
            DataExclusao = produto.DataExclusao,
            Ativo = produto.Ativo,
            Categoria = (int)produto.Categoria,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco
        };
    }
}
