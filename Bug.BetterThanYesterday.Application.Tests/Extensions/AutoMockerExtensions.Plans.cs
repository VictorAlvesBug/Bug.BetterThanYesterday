using Bug.BetterThanYesterday.Application.Tests.Commons;
using Moq.AutoMock;

namespace Bug.BetterThanYesterday.Application.Tests.Extensions;

public static partial class AutoMockerExtensions
{
    public static MockedValues SetupForPlans(this AutoMocker mocker, MockedValues? mockedValues = null)
    {
        mockedValues ??= new MockedValues();

        (mockedValues.PlanRepository, mockedValues.Plans) = PlanRepositoryMockFactory.Create();
        mocker.Use(mockedValues.PlanRepository.Object);
        mocker.Use(mockedValues.Plans);

        (mockedValues.HabitRepository, mockedValues.Habits) = HabitRepositoryMockFactory.Create();
        mocker.Use(mockedValues.HabitRepository.Object);
        mocker.Use(mockedValues.Habits);

        return mockedValues;
    }
}