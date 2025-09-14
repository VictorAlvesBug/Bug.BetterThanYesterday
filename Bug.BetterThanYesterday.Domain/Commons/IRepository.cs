namespace Bug.BetterThanYesterday.Domain.Commons
{
	public interface IRepository<TEntity> where TEntity : Entity
	{
		Task<List<TEntity>> ListAllAsync();
		Task<TEntity> GetByIdAsync(string id);
		Task AddAsync(TEntity entity);
		Task UpdateAsync(TEntity entity);
		Task DeleteAsync(TEntity entity);
	}
}
