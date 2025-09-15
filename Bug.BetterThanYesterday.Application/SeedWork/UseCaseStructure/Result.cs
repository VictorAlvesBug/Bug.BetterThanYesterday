namespace Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;

public sealed class Result<TData>
{
	public ResultStatus Status { get; }
	public string Reason { get; }
	public TData? Data { get; }

	private Result(ResultStatus status, string reason, TData? data = default)
	{
		Status = status;
		Reason = reason;
		Data = data;
	}

	public static Result<TData> Success(TData data, string reason)
		=> new(ResultStatus.Success, reason, data);

	public static Result<TData> Success(TData data)
		=> new(ResultStatus.Success, string.Empty, data);

	public static Result<TData> Success(string reason)
		=> new(ResultStatus.Success, reason);

	public static Result<TData> Failure(TData data, string reason)
		=> new(ResultStatus.Failure, reason, data);

	public static Result<TData> Failure(string reason)
		=> new(ResultStatus.Failure, reason);

	public static Result<TData> Rejected(TData data, string reason)
		=> new(ResultStatus.Rejected, reason, data);

	public static Result<TData> Rejected(string reason)
		=> new(ResultStatus.Rejected, reason);

	public bool IsSuccess() => Status == ResultStatus.Success;
	public bool IsRejected() => Status == ResultStatus.Rejected;
}
