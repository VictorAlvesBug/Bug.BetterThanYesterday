namespace Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;

public interface IUseCase<in TInput, TResult>
	where TInput : ICommand
	where TResult : IResult
{
	Task<TResult> HandleAsync(TInput input);
}
