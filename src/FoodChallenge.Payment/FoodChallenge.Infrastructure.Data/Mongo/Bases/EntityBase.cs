using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FoodChallenge.Infrastructure.Data.Mongo.Bases;

public class EntityBase
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid? Id { get; set; }

    [BsonElement("dataCriacao")]
    public DateTime DataCriacao { get; set; }

    [BsonElement("dataAtualizacao")]
    public DateTime? DataAtualizacao { get; set; }

    [BsonElement("ativo")]
    public bool Ativo { get; set; }

    [BsonElement("dataExclusao")]
    public DateTime? DataExclusao { get; set; }
}

