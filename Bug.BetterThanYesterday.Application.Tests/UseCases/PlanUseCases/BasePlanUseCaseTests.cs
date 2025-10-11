using Bug.BetterThanYesterday.Application.Tests.Extensions;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanUseCases;

public class BasePlanUseCaseTests : BaseUseCaseTests
{
	public BasePlanUseCaseTests()
	{
		_mock = _mocker.SetupForPlans();
	}
}
