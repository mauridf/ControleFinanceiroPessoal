using ControleFinanceiroPessoal.Entities;
using ControleFinanceiroPessoal.Infrastructure;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ControleFinanceiroPessoal.Repositories;

public class GenericRepository<T> where T : class, IEntity
{
    protected readonly IMongoCollection<T> _collection;

    public GenericRepository(IMongoClient client, IOptions<MongoDbSettings> settings)
    {
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _collection = database.GetCollection<T>(typeof(T).Name.ToLower());
    }

    public IMongoCollection<T> GetCollection() => _collection;

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<T> GetByIdAsync(ObjectId id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task InsertAsync(T entity) =>
        await _collection.InsertOneAsync(entity);

    public async Task UpdateAsync(T entity) =>
        await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);

    public async Task DeleteAsync(ObjectId id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);

    public async Task<IEnumerable<T>> FindByConditionAsync(FilterDefinition<T> filter) =>
    await _collection.Find(filter).ToListAsync();

    public async Task<IEnumerable<BsonDocument>> AggregateAsync(PipelineDefinition<T, BsonDocument> pipeline)
    {
        using var cursor = await _collection.AggregateAsync(pipeline);
        return await cursor.ToListAsync();
    }
}
