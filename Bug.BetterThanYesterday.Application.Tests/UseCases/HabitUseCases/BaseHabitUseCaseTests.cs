using Bug.BetterThanYesterday.Application.Tests.Extensions;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.HabitUseCases;

public class BaseHabitUseCaseTests : BaseUseCaseTests
{
	public BaseHabitUseCaseTests()
	{
		_mock = _mocker.SetupForHabits();
	}
}
