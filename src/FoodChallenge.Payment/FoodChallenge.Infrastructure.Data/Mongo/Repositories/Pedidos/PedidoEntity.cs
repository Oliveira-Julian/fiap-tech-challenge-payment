using FoodChallenge.Infrastructure.Data.Mongo.Bases;
using MongoDB.Bson.Serialization.Attributes;

namespace FoodChallenge.Infrastructure.Data.Mongo.Repositories.Pedidos;

[BsonIgnoreExtraElements]
public class PedidoEntity : EntityBase
{
    [BsonElement("idCliente")]
    public Guid? IdCliente { get; set; }

    [BsonElement("idPagamento")]
    public Guid? IdPagamento { get; set; }

    [BsonElement("itens")]
    public List<PedidoItemEntity> Itens { get; set; } = new();

    [BsonElement("codigo")]
    public string Codigo { get; set; }

    [BsonElement("valorTotal")]
    public decimal ValorTotal { get; set; }

    [BsonElement("status")]
    public int Status { get; set; }
}

