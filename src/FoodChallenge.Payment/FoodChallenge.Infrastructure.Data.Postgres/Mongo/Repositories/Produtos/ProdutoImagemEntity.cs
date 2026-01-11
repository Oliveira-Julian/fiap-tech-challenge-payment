using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Produtos;

[BsonIgnoreExtraElements]
public class ProdutoImagemEntity
{
    [BsonElement("id")]
    [BsonRepresentation(BsonType.String)]
    public Guid? Id { get; set; }

    [BsonElement("idProduto")]
    [BsonRepresentation(BsonType.String)]
    public Guid? IdProduto { get; set; }

    [BsonElement("nome")]
    public string Nome { get; set; }

    [BsonElement("tipo")]
    public string Tipo { get; set; }

    [BsonElement("tamanho")]
    public decimal Tamanho { get; set; }

    [BsonElement("conteudo")]
    public byte[] Conteudo { get; set; }

    [BsonElement("dataCriacao")]
    public DateTime DataCriacao { get; set; }

    [BsonElement("dataAtualizacao")]
    public DateTime? DataAtualizacao { get; set; }

    [BsonElement("ativo")]
    public bool Ativo { get; set; }

    [BsonElement("dataExclusao")]
    public DateTime? DataExclusao { get; set; }
}
