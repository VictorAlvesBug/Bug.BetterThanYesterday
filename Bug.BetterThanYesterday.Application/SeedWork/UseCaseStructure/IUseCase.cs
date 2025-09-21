namespace Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;

public interface IUseCase<in TCommand, TResult>
	where TCommand : ICommand
	where TResult : IResult
{
	Task<TResult> HandleAsync(TCommand command);
}
