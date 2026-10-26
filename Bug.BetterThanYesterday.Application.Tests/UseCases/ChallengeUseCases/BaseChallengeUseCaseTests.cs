using Bug.BetterThanYesterday.Application.Tests.Extensions;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.ChallengeUseCases;

public class BaseChallengeUseCaseTests : BaseUseCaseTests
{
	public BaseChallengeUseCaseTests()
	{
		_mock = _mocker.SetupForChallenges();
	}
}
