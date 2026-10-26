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

        (mockedValues.ChallengeMemberRepository, mockedValues.ChallengeMembers) = ChallengeMemberRepositoryMockFactory.Create();
        mocker.Use(mockedValues.ChallengeMemberRepository.Object);
        mocker.Use(mockedValues.ChallengeMembers);

        (mockedValues.HabitRepository, mockedValues.Habits) = HabitRepositoryMockFactory.Create();
        mocker.Use(mockedValues.HabitRepository.Object);
        mocker.Use(mockedValues.Habits);

        (mockedValues.ChallengeRepository, mockedValues.Challenges) = ChallengeRepositoryMockFactory.Create();
        mocker.Use(mockedValues.ChallengeRepository.Object);
        mocker.Use(mockedValues.Challenges);

        (mockedValues.UserRepository, mockedValues.Users) = UserRepositoryMockFactory.Create();
        mocker.Use(mockedValues.UserRepository.Object);
        mocker.Use(mockedValues.Users);

        return mockedValues;
    }
}