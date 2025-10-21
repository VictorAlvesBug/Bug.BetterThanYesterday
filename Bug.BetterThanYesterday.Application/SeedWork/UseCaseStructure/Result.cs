namespace Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;

public class Result : IResult
{
	public ResultStatus Status { get; }
	public string Reason { get; }

	protected Result(ResultStatus status, string reason)
	{
		Status = status;
		Reason = reason;
	}

	public static Result Success<TData>(TData data, string reason)
		=> new Result<TData>(ResultStatus.Success, reason, data);

	public static Result Success<TData>(TData data)
		=> new Result<TData>(ResultStatus.Success, string.Empty, data);

	public static Result Success(string reason)
		=> new(ResultStatus.Success, reason);

	public static Result Failure<TData>(TData data, string reason)
		=> new Result<TData>(ResultStatus.Failure, reason, data);

	public static Result Failure(string reason)
		=> new(ResultStatus.Failure, reason);

	public static Result Rejected<TData>(TData data, string reason)
		=> new Result<TData>(ResultStatus.Rejected, reason, data);

	public static Result Rejected(string reason)
		=> new(ResultStatus.Rejected, reason);

	public bool IsSuccess() => Status == ResultStatus.Success;
	public bool IsRejected() => Status == ResultStatus.Rejected;
	public bool IsFailure() => Status == ResultStatus.Failure;
	public string GetMessage() => Reason;
}

public sealed class Result<TData> : Result
{
	public TData Data { get; }

	public Result(ResultStatus status, string reason, TData data)
		: base(status, reason) => Data = data;
}
