using FoodChallenge.Infrastructure.Data.Mongo.Bases;
using MongoDB.Bson.Serialization.Attributes;

namespace FoodChallenge.Infrastructure.Data.Mongo.Repositories.Clientes;

[BsonIgnoreExtraElements]
public sealed class ClienteEntity : EntityBase
{
    [BsonElement("cpf")]
    public string Cpf { get; set; }

    [BsonElement("nome")]
    public string Nome { get; set; }

    [BsonElement("email")]
    public string Email { get; set; }
}

