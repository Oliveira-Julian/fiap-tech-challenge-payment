using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Bases;
using MongoDB.Bson.Serialization.Attributes;

namespace FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Pedidos;

[BsonIgnoreExtraElements]
public class PagamentoEntity : EntityBase
{
    [BsonElement("idPedido")]
    public Guid? IdPedido { get; set; }

    [BsonElement("chaveMercadoPagoOrdem")]
    public Guid? ChaveMercadoPagoOrdem { get; set; }

    [BsonElement("idMercadoPagoOrdem")]
    public string IdMercadoPagoOrdem { get; set; }

    [BsonElement("idMercadoPagoPagamento")]
    public string IdMercadoPagoPagamento { get; set; }

    [BsonElement("status")]
    public int Status { get; set; }

    [BsonElement("valor")]
    public decimal Valor { get; set; }

    [BsonElement("metodo")]
    public int Metodo { get; set; }

    [BsonElement("qrCode")]
    public string QrCode { get; set; }
}
