namespace Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;

public class Result : IResult
{
	public ResultStatus Status { get; }
	public string Reason { get; }
	public RejectionType RejectionType { get; }

	protected Result(ResultStatus status, string reason, RejectionType rejectionType = default)
	{
		Status = status;
		Reason = reason;
		RejectionType = rejectionType;
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

	public static Result Rejected(string reason)
		=> new(ResultStatus.Rejected, reason);

	public static Result Rejected(string reason, RejectionType rejectionType)
		=> new(ResultStatus.Rejected, reason, rejectionType);

	public bool IsSuccess() => Status == ResultStatus.Success;
	public bool IsRejected() => Status == ResultStatus.Rejected;
	public bool IsFailure() => Status == ResultStatus.Failure;
	public string GetMessage() => Reason;

	public int GetStatusCode()
	{
		if (RejectionType != default)
			return (int) RejectionType;

		return Status switch
		{
			ResultStatus.Rejected => (int) RejectionType.BadRequest,
			ResultStatus.Failure => (int) RejectionType.InternalServerError,
			_ => 200
		};
	}
}

public sealed class Result<TData> : Result
{
	public TData Data { get; }

	public Result(ResultStatus status, string reason, TData data)
		: base(status, reason) => Data = data;
}
