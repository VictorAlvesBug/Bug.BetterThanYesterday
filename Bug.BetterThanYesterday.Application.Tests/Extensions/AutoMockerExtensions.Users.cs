using Bug.BetterThanYesterday.Application.Tests.Commons;
using Moq.AutoMock;

namespace Bug.BetterThanYesterday.Application.Tests.Extensions;

public static partial class AutoMockerExtensions
{
    public static MockedValues SetupForUsers(this AutoMocker mocker, MockedValues? mockedValues = null)
    {
        mockedValues ??= new MockedValues();

        (mockedValues.UserRepository, mockedValues.Users) = UserRepositoryMockFactory.Create();
        mocker.Use(mockedValues.UserRepository.Object);
        mocker.Use(mockedValues.Users);

        return mockedValues;
    }
}