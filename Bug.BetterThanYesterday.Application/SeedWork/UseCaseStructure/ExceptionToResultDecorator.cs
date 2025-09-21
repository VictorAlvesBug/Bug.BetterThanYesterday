namespace Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;

public sealed class ExceptionToResultDecorator<TCommand>(IUseCase<TCommand> inner)
	: IUseCase<TCommand>
	where TCommand : ICommand
{
	public async Task<IResult> HandleAsync(TCommand command)
	{
		try
		{
			return await inner.HandleAsync(command);
		}
		catch (Exception ex)
		{
			if (ex is ArgumentException
			|| ex is ArgumentNullException
			|| ex is ArgumentOutOfRangeException
			|| ex is InvalidOperationException)
			{
				return Result.Rejected(ex.Message);
			}

			return Result.Failure(ex.Message);
		}
	}
}
