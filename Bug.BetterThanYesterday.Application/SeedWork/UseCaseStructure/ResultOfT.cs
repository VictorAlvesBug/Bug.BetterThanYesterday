namespace Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;

public sealed class Result<TData> : Result
{
	public TData Data { get; }

	public Result(ResultStatus status, string reason, TData data)
		: base(status, reason) => Data = data;
}
