namespace Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;

public interface IUseCase<in TCommand>
	where TCommand : ICommand
{
	Task<IResult> HandleAsync(TCommand command);
}
