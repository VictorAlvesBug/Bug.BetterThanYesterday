using Bug.BetterThanYesterday.Domain.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

public class Repository<TEntity, TDocument> : IRepository<TEntity>
	where TEntity : Entity
	where TDocument : Document
{
	protected readonly IMongoCollection<TDocument> _collection;
	protected readonly IDocumentMapper<TEntity, TDocument> _mapper;

	public Repository(
		IMongoCollection<TDocument> collection,
		IDocumentMapper<TEntity, TDocument> mapper)
	{
		_collection = collection;
		_mapper = mapper;
	}

	public async Task<List<TEntity>> ListAllAsync()
	{
		var documents = (await _collection.FindAsync(_ => true)).ToList();
		return documents.ConvertAll(_mapper.ToDomain);
	}

	public async Task<TEntity?> GetByIdAsync(Guid id)
	{
		var document = (await _collection.FindAsync(doc => doc.Id == id)).FirstOrDefault();
		return document is null ? null : _mapper.ToDomain(document);
	}

	public async Task<List<TEntity>> BatchGetByIdAsync(List<Guid> ids)
	{
		var documents = new List<TDocument>();

		foreach (var chunk in ids.Chunk(1000))
		{
			var filter = Builders<TDocument>.Filter.In(doc => doc.Id, chunk);
			documents.AddRange((await _collection.FindAsync(filter)).ToList());
		}

		return documents.ConvertAll(_mapper.ToDomain);
	}

	public async Task AddAsync(TEntity entity)
	{
		await _collection.InsertOneAsync(_mapper.ToDocument(entity));
	}

	public async Task ReplaceAsync(TEntity entity)
	{
		await _collection.ReplaceOneAsync(
			filter: doc => doc.Id == entity.Id, 
			replacement: _mapper.ToDocument(entity),
			options: new ReplaceOptions { IsUpsert = true }
		);
	}

	public async Task UpdateAsync(TEntity entity)
	{
		await _collection.ReplaceOneAsync(doc => doc.Id == entity.Id, _mapper.ToDocument(entity));
	}

	public async Task DeleteAsync(TEntity entity)
	{
		await _collection.DeleteOneAsync(doc => doc.Id == entity.Id);
	}

	public async Task DeleteManyAsync(List<TEntity> entities)
	{
		var idsToDelete = entities.Select(entity => entity.Id).ToHashSet();
		await _collection.DeleteManyAsync(doc => idsToDelete.Contains(doc.Id));
	}

	public async Task DeleteAllAsync()
	{
		var filter = Builders<TDocument>.Filter.Empty;
		await _collection.DeleteManyAsync(filter);
	}
}
