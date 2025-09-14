using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Configurations;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Commons
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
	{
		protected readonly IMongoCollection<TEntity> _entities;

		public Repository(IDatabaseConfig databaseConfig, string entityCollection)
		{
			var client = new MongoClient(databaseConfig.ConnectionString);
			var database = client.GetDatabase(databaseConfig.DatabaseName);
			_entities = database.GetCollection<TEntity>(entityCollection);
		}

		public async Task AddAsync(TEntity entity)
		{
			await _entities.InsertOneAsync(entity);
		}

		public async Task DeleteAsync(TEntity entity)
		{
			await _entities.DeleteOneAsync(u => u.Id == entity.Id);
		}

		public async Task<List<TEntity>> GetAllAsync()
		{
			return (await _entities.FindAsync(_ => true)).ToList();
		}

		public async Task<TEntity> GetByIdAsync(string id)
		{
			return (await _entities.FindAsync(entity => entity.Id == id)).FirstOrDefault();
		}

		public async Task UpdateAsync(TEntity entity)
		{
			await _entities.ReplaceOneAsync(u => u.Id == entity.Id, entity);
		}
	}
}
