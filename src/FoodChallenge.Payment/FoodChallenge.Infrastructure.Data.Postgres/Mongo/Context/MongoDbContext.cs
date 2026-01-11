using MongoDB.Driver;

namespace FoodChallenge.Infrastructure.Data.Postgres.Mongo.Context;

public sealed class MongoDbContext
{
    private readonly IMongoDatabase _database;
    public IMongoClient Client { get; }

    public MongoDbContext(string connectionString, string databaseName)
    {
        Client = new MongoClient(connectionString);
        _database = Client.GetDatabase(databaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }
}
