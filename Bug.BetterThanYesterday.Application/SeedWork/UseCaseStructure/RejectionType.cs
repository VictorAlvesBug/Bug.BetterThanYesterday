namespace Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure
{
	public enum RejectionType
	{
		BadRequest = 400,
		Unauthorized = 401,
		PaymentRequired = 402,
		Forbidden = 403,
		NotFound = 404,
		MethodNotAllowed = 405,
		RequestTimeout = 408,
		//UnsupportedMediaType = 415,
		InternalServerError = 500
	}
}
