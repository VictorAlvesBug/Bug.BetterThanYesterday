using Bug.BetterThanYesterday.Application.Tests.Commons;
using Moq.AutoMock;

namespace Bug.BetterThanYesterday.Application.Tests.Extensions;

public static partial class AutoMockerExtensions
{
    public static MockedValues SetupForPlanMembers(this AutoMocker mocker, MockedValues? mockedValues = null)
    {
        mockedValues ??= new MockedValues();

        (mockedValues.PlanMemberRepository, mockedValues.PlanMembers) = PlanMemberRepositoryMockFactory.Create();
        mocker.Use(mockedValues.PlanMemberRepository.Object);
        mocker.Use(mockedValues.PlanMembers);

        (mockedValues.PlanRepository, mockedValues.Plans) = PlanRepositoryMockFactory.Create();
        mocker.Use(mockedValues.PlanRepository.Object);
        mocker.Use(mockedValues.Plans);

        (mockedValues.UserRepository, mockedValues.Users) = UserRepositoryMockFactory.Create();
        mocker.Use(mockedValues.UserRepository.Object);
        mocker.Use(mockedValues.Users);

        return mockedValues;
    }
}