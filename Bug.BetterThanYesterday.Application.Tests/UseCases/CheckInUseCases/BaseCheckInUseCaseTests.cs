using Bug.BetterThanYesterday.Application.Tests.Extensions;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.CheckInUseCases;

public class BaseCheckInUseCaseTests : BaseUseCaseTests
{
    public BaseCheckInUseCaseTests()
    {
        _mock = _mocker.SetupForCheckIns();
    }
}