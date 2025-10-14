using Bug.BetterThanYesterday.Application.Habits.DeleteHabit;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Habits.Policies;
using Moq.AutoMock;

namespace Bug.BetterThanYesterday.Application.Tests.Extensions;

public static class AutoMockerExtensions
{
	public static MockedValues SetupForUsers(this AutoMocker mocker, MockedValues? mockedValues = null)
	{
		mockedValues ??= new MockedValues();

		(mockedValues.UserRepository, mockedValues.Users) = UserRepositoryMockFactory.Create();
		mocker.Use(mockedValues.UserRepository.Object);
		mocker.Use(mockedValues.Users);

		return mockedValues;
	}

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

	public static MockedValues SetupForPlanParticipants(this AutoMocker mocker, MockedValues? mockedValues = null)
	{
		mockedValues ??= new MockedValues();

		(mockedValues.PlanParticipantRepository, mockedValues.PlanParticipants) = PlanParticipantRepositoryMockFactory.Create();
		mocker.Use(mockedValues.PlanParticipantRepository.Object);
		mocker.Use(mockedValues.PlanParticipants);

		(mockedValues.PlanRepository, mockedValues.Plans) = PlanRepositoryMockFactory.Create();
		mocker.Use(mockedValues.PlanRepository.Object);
		mocker.Use(mockedValues.Plans);

		(mockedValues.UserRepository, mockedValues.Users) = UserRepositoryMockFactory.Create();
		mocker.Use(mockedValues.UserRepository.Object);
		mocker.Use(mockedValues.Users);

		return mockedValues;
	}
}
