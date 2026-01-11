using FoodChallenge.CrossCutting.Paging;
using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FoodChallenge.Infrastructure.Data.Postgres.Mongo.Context;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
{
    protected readonly IMongoCollection<TEntity> _collection;

    protected RepositoryBase(MongoDbContext context, string collectionName)
    {
        _collection = context.GetCollection<TEntity>(collectionName);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        return entity;
    }

    public async Task AddItemsAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await _collection.InsertManyAsync(entities, cancellationToken: cancellationToken);
    }

    public void Update(TEntity entity)
    {
        entity.DataAtualizacao = DateTime.UtcNow;
        _collection.ReplaceOne(e => e.Id == entity.Id, entity);
    }

    public void UpdateItems(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.DataAtualizacao = DateTime.UtcNow;
            _collection.ReplaceOne(e => e.Id == entity.Id, entity);
        }
    }

    public void Remove(TEntity entity)
    {
        _collection.DeleteOne(e => e.Id == entity.Id);
    }

    public void RemoveItems(IEnumerable<TEntity> entities)
    {
        var ids = entities.Select(e => e.Id).ToList();
        _collection.DeleteMany(e => ids.Contains(e.Id));
    }

    public virtual IQueryable<TEntity> GetQuery(bool tracking = false)
    {
        return _collection.AsQueryable();
    }

    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool tracking = false)
    {
        return await _collection.Find(e => e.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<TEntity>> GetByIdsAsync(IEnumerable<Guid?> ids, CancellationToken cancellationToken, bool tracking = false)
    {
        var idList = ids.ToList();
        return await _collection.Find(e => idList.Contains(e.Id)).ToListAsync(cancellationToken);
    }

    public async Task<Pagination<TEntity>> QueryPagedAsync<TFilter>(Filter<TFilter> filter, CancellationToken cancellationToken)
        where TFilter : class, new()
    {
        var filterDefinition = BuildFilterDefinition(filter.Fields);

        var sortDefinition = BuildSortDefinition(filter.FieldOrdenation, filter.OrdenationAsc);

        var total = await _collection.CountDocumentsAsync(filterDefinition, cancellationToken: cancellationToken);

        var items = await _collection
            .Find(filterDefinition)
            .Sort(sortDefinition)
            .Skip((filter.Page - 1) * filter.SizePage)
            .Limit(filter.SizePage)
            .ToListAsync(cancellationToken);

        return new Pagination<TEntity>(filter.Page, items.Count, (int)total, items);
    }

    protected static FilterDefinition<TEntity> BuildFilterDefinition<TFilter>(TFilter filterFields) where TFilter : class
    {
        var builder = Builders<TEntity>.Filter;
        var filters = new List<FilterDefinition<TEntity>>();

        if (filterFields == null)
            return builder.Empty;

        foreach (var filterProp in typeof(TFilter).GetProperties())
        {
            if (filterProp.GetCustomAttribute<IgnoreFilterAttribute>() != null)
                continue;

            var value = filterProp.GetValue(filterFields);
            if (value == null || value.Equals(GetDefault(value.GetType())))
                continue;

            var filterAttr = filterProp.GetCustomAttribute<FilterByAttribute>();
            var entityPropName = filterAttr?.PropertyName ?? filterProp.Name;
            var filterType = filterAttr?.FilterType ?? FilterType.Equals;

            var filter = BuildFilter(builder, entityPropName, value, filterType);
            if (filter != null)
                filters.Add(filter);
        }

        return filters.Any() ? builder.And(filters) : builder.Empty;
    }

    private static FilterDefinition<TEntity> BuildFilter(FilterDefinitionBuilder<TEntity> builder, string propertyName, object value, FilterType filterType)
    {
        return filterType switch
        {
            FilterType.Contains when value is string strValue => builder.Regex(propertyName, new MongoDB.Bson.BsonRegularExpression(strValue, "i")),
            FilterType.GreaterThan => builder.Gt(propertyName, value),
            FilterType.LessThan => builder.Lt(propertyName, value),
            FilterType.GreaterOrEqual => builder.Gte(propertyName, value),
            FilterType.LessOrEqual => builder.Lte(propertyName, value),
            FilterType.NotEqual => builder.Ne(propertyName, value),
            _ when value is System.Collections.IEnumerable enumerable && value is not string => builder.In(propertyName, enumerable.Cast<object>()),
            _ => builder.Eq(propertyName, value)
        };
    }

    protected static SortDefinition<TEntity> BuildSortDefinition(string fieldOrdenation, bool ascending)
    {
        var builder = Builders<TEntity>.Sort;

        if (string.IsNullOrEmpty(fieldOrdenation))
            return builder.Descending(e => e.DataCriacao);

        return ascending
            ? builder.Ascending(fieldOrdenation)
            : builder.Descending(fieldOrdenation);
    }

    private static object GetDefault(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}
