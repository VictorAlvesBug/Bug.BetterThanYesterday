namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;

public interface IDocumentMapper<TEntity, TDocument>
{
	TDocument ToDocument(TEntity entity);
	TEntity ToDomain(TDocument document);
}
