namespace Bug.BetterThanYesterday.Application.SeedWork;

public interface IModelMapper<TEntity, TModel>
{
	TModel ToModel(TEntity entity);
}
