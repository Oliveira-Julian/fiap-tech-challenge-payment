using FoodChallenge.Common.Interfaces;
using MongoDB.Driver;

namespace FoodChallenge.Infrastructure.Data.Mongo.Context;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private IClientSessionHandle _session;
    private readonly IMongoClient _client;
    private bool _disposed = false;

    public UnitOfWork(MongoDbContext context)
    {
        _client = context.Client ?? throw new ArgumentNullException(nameof(context));
    }

    public void BeginTransaction()
    {
        _session = _client.StartSession();
        _session.StartTransaction();
    }

    public async Task CommitAsync()
    {
        if (_session != null && _session.IsInTransaction)
            await _session.CommitTransactionAsync();
    }

    public async Task RollbackAsync()
    {
        try
        {
            if (_session != null && _session.IsInTransaction)
                await _session.AbortTransactionAsync();
        }
        catch { /* ignora caso ocorra alguma exceção */ }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed || !disposing)
            return;

        try
        {
            _session?.Dispose();
        }
        catch { /* ignora caso ocorra alguma exceção */ }

        _disposed = true;
    }
}

