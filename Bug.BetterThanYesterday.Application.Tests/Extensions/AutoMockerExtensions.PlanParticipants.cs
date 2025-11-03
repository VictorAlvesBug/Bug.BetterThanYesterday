using Bug.BetterThanYesterday.Application.Tests.Commons;
using Moq.AutoMock;

namespace Bug.BetterThanYesterday.Application.Tests.Extensions;

public static partial class AutoMockerExtensions
{
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