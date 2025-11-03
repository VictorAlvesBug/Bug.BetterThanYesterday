using Bug.BetterThanYesterday.Application.Habits.DeleteHabit;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Habits.Policies;
using Moq.AutoMock;

namespace Bug.BetterThanYesterday.Application.Tests.Extensions;

public static partial class AutoMockerExtensions
{
    public static MockedValues SetupForHabits(this AutoMocker mocker, MockedValues? mockedValues = null)
    {
        mockedValues ??= new MockedValues();

        (mockedValues.HabitRepository, mockedValues.Habits) = HabitRepositoryMockFactory.Create();
        mocker.Use(mockedValues.HabitRepository.Object);
        mocker.Use(mockedValues.Habits);

        (mockedValues.PlanRepository, mockedValues.Plans) = PlanRepositoryMockFactory.Create();
        mocker.Use(mockedValues.PlanRepository.Object);
        mocker.Use(mockedValues.Plans);

        var habitDeletionPolicy = new HabitDeletionPolicy(mockedValues.PlanRepository.Object);
        mocker.Use<IHabitDeletionPolicy>(habitDeletionPolicy);

        return mockedValues;
    }
}