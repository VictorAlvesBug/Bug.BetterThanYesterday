using Bug.BetterThanYesterday.Application.Tests.Extensions;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.UserUseCases;

public class BaseUserUseCaseTests : BaseUseCaseTests
{
	public BaseUserUseCaseTests()
	{
		_mock = _mocker.SetupForUsers();
	}
}
