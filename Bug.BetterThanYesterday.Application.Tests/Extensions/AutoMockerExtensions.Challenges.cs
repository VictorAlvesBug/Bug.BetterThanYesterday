using Bug.BetterThanYesterday.Application.Tests.Commons;
using Moq.AutoMock;

namespace Bug.BetterThanYesterday.Application.Tests.Extensions;

public static partial class AutoMockerExtensions
{
    public static MockedValues SetupForChallenges(this AutoMocker mocker, MockedValues? mockedValues = null)
    {
        mockedValues ??= new MockedValues();

        (mockedValues.ChallengeRepository, mockedValues.Challenges) = ChallengeRepositoryMockFactory.Create();
        mocker.Use(mockedValues.ChallengeRepository.Object);
        mocker.Use(mockedValues.Challenges);

        (mockedValues.HabitRepository, mockedValues.Habits) = HabitRepositoryMockFactory.Create();
        mocker.Use(mockedValues.HabitRepository.Object);
        mocker.Use(mockedValues.Habits);

        (mockedValues.UserRepository, mockedValues.Users) = UserRepositoryMockFactory.Create();
        mocker.Use(mockedValues.UserRepository.Object);
        mocker.Use(mockedValues.Users);

        (mockedValues.ChallengeMemberRepository, mockedValues.ChallengeMembers) = ChallengeMemberRepositoryMockFactory.Create();
        mocker.Use(mockedValues.ChallengeMemberRepository.Object);
        mocker.Use(mockedValues.ChallengeMembers);

        return mockedValues;
    }
}