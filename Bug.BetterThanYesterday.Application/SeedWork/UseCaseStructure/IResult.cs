namespace Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;

public interface IResult
{

	bool IsSuccess();
	bool IsRejected();
	bool IsFailure();
	string GetMessage();
}