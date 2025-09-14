namespace Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure
{
	public interface IUseCase<in TInput, TResult>
	{
		Task<TResult> HandleAsync(TInput input);
	}
}
