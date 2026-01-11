using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FoodChallenge.Infrastructure.Data.Mongo.Repositories.Pedidos;

[BsonIgnoreExtraElements]
public class PedidoItemEntity
{
    [BsonElement("id")]
    [BsonRepresentation(BsonType.String)]
    public Guid? Id { get; set; }

    [BsonElement("idProduto")]
    [BsonRepresentation(BsonType.String)]
    public Guid? IdProduto { get; set; }

    [BsonElement("idPedido")]
    [BsonRepresentation(BsonType.String)]
    public Guid? IdPedido { get; set; }

    [BsonElement("codigo")]
    public string Codigo { get; set; }

    [BsonElement("valor")]
    public decimal Valor { get; set; }

    [BsonElement("quantidade")]
    public int Quantidade { get; set; }

    [BsonElement("dataCriacao")]
    public DateTime DataCriacao { get; set; }

    [BsonElement("dataAtualizacao")]
    public DateTime? DataAtualizacao { get; set; }

    [BsonElement("ativo")]
    public bool Ativo { get; set; }

    [BsonElement("dataExclusao")]
    public DateTime? DataExclusao { get; set; }
}

