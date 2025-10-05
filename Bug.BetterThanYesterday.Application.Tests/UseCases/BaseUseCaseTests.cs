using Bug.BetterThanYesterday.Application.Tests.Commons;
using Moq.AutoMock;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases
{
	public class BaseUseCaseTests
	{
		protected readonly AutoMocker _mocker = new();
		protected MockedValues _mock;
	}
}
