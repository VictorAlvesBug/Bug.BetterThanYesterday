using Bug.BetterThanYesterday.Application.Tests.Extensions;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanMemberUseCases;

public class BasePlanMemberUseCaseTests : BaseUseCaseTests
{
	public BasePlanMemberUseCaseTests()
	{
		_mock = _mocker.SetupForPlanMembers();
	}
}
