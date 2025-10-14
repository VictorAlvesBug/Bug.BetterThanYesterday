using Bug.BetterThanYesterday.Application.Tests.Extensions;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanParticipantUseCases;

public class BasePlanParticipantUseCaseTests : BaseUseCaseTests
{
	public BasePlanParticipantUseCaseTests()
	{
		_mock = _mocker.SetupForPlanParticipants();
	}
}
