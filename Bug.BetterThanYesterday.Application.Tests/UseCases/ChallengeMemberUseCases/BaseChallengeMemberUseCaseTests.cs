using Bug.BetterThanYesterday.Application.Tests.Extensions;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.ChallengeMemberUseCases;

public class BaseChallengeMemberUseCaseTests : BaseUseCaseTests
{
	public BaseChallengeMemberUseCaseTests()
	{
		_mock = _mocker.SetupForChallengeMembers();
	}
}
