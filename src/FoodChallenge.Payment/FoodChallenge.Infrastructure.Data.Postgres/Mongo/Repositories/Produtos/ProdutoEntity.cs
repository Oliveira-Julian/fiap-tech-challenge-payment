using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Bases;
using MongoDB.Bson.Serialization.Attributes;

namespace FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Produtos;

[BsonIgnoreExtraElements]
public sealed class ProdutoEntity : EntityBase
{
    [BsonElement("categoria")]
    public int Categoria { get; set; }

    [BsonElement("nome")]
    public string Nome { get; set; }

    [BsonElement("descricao")]
    public string Descricao { get; set; }

    [BsonElement("preco")]
    public decimal Preco { get; set; }

    [BsonElement("imagens")]
    public List<ProdutoImagemEntity> Imagens { get; set; } = new();
}
