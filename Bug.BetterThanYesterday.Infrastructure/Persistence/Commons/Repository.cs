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

	public async Task AddAsync(TEntity entity)
	{
		await _collection.InsertOneAsync(_mapper.ToDocument(entity));
	}

	public async Task DeleteAsync(TEntity entity)
	{
		await _collection.DeleteOneAsync(u => u.Id == entity.Id);
	}

	public async Task<List<TEntity>> ListAllAsync()
	{
		var documents = (await _collection.FindAsync(_ => true)).ToList();
		return documents.ConvertAll(_mapper.ToDomain);
	}

	public async Task<TEntity?> GetByIdAsync(string id)
	{
		var document = (await _collection.FindAsync(entity => entity.Id == id)).FirstOrDefault();
		return document is null ? null : _mapper.ToDomain(document);
	}

	public async Task UpdateAsync(TEntity entity)
	{
		await _collection.ReplaceOneAsync(u => u.Id == entity.Id, _mapper.ToDocument(entity));
	}
}
