using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Bases;
using MongoDB.Bson.Serialization.Attributes;

namespace FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Preparos;

[BsonIgnoreExtraElements]
public class OrdemPedidoEntity : EntityBase
{
    [BsonElement("idPedido")]
    public Guid? IdPedido { get; set; }

    [BsonElement("status")]
    public int Status { get; set; }

    [BsonElement("dataInicioPreparacao")]
    public DateTime? DataInicioPreparacao { get; set; }

    [BsonElement("dataFimPreparacao")]
    public DateTime? DataFimPreparacao { get; set; }
}
