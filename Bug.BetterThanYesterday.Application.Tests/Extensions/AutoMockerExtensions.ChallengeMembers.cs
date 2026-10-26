using Bug.BetterThanYesterday.Application.Tests.Commons;
using Bug.BetterThanYesterday.Domain.Penalties;
using Moq;
using Moq.AutoMock;

namespace Bug.BetterThanYesterday.Application.Tests.Extensions;

public static partial class AutoMockerExtensions
{
    public static MockedValues SetupForChallengeMembers(this AutoMocker mocker, MockedValues? mockedValues = null)
    {
        mockedValues ??= new MockedValues();

        (mockedValues.ChallengeMemberRepository, mockedValues.ChallengeMembers) = ChallengeMemberRepositoryMockFactory.Create();
        mocker.Use(mockedValues.ChallengeMemberRepository.Object);
        mocker.Use(mockedValues.ChallengeMembers);

        (mockedValues.ChallengeRepository, mockedValues.Challenges) = ChallengeRepositoryMockFactory.Create();
        mocker.Use(mockedValues.ChallengeRepository.Object);
        mocker.Use(mockedValues.Challenges);

        (mockedValues.HabitRepository, mockedValues.Habits) = HabitRepositoryMockFactory.Create();
        mocker.Use(mockedValues.HabitRepository.Object);
        mocker.Use(mockedValues.Habits);

        (mockedValues.UserRepository, mockedValues.Users) = UserRepositoryMockFactory.Create();
        mocker.Use(mockedValues.UserRepository.Object);
        mocker.Use(mockedValues.Users);

        var penaltyRepository = new Mock<IPenaltyRepository>();
        penaltyRepository
            .Setup(repository => repository.ListByChallengeIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync([]);
        mocker.Use(penaltyRepository.Object);

        return mockedValues;
    }
}