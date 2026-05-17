using Bug.BetterThanYesterday.Application.Tests.Commons;
using Moq.AutoMock;

namespace Bug.BetterThanYesterday.Application.Tests.Extensions;

public static partial class AutoMockerExtensions
{
    public static MockedValues SetupForCheckIns(this AutoMocker mocker, MockedValues? mockedValues = null)
    {
        mockedValues ??= new MockedValues();

        (mockedValues.CheckInRepository, mockedValues.CheckIns) = CheckInRepositoryMockFactory.Create();
        mocker.Use(mockedValues.CheckInRepository.Object);
        mocker.Use(mockedValues.CheckIns);

        (mockedValues.PlanMemberRepository, mockedValues.PlanMembers) = PlanMemberRepositoryMockFactory.Create();
        mocker.Use(mockedValues.PlanMemberRepository.Object);
        mocker.Use(mockedValues.PlanMembers);

        (mockedValues.HabitRepository, mockedValues.Habits) = HabitRepositoryMockFactory.Create();
        mocker.Use(mockedValues.HabitRepository.Object);
        mocker.Use(mockedValues.Habits);

        (mockedValues.PlanRepository, mockedValues.Plans) = PlanRepositoryMockFactory.Create();
        mocker.Use(mockedValues.PlanRepository.Object);
        mocker.Use(mockedValues.Plans);

        (mockedValues.UserRepository, mockedValues.Users) = UserRepositoryMockFactory.Create();
        mocker.Use(mockedValues.UserRepository.Object);
        mocker.Use(mockedValues.Users);

        return mockedValues;
    }
}