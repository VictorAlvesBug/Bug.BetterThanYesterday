using Bug.BetterThanYesterday.Application.Plans.ListAllPlans;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Moq;
using Xunit;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanUseCases;

public class ListAllPlansUseCaseTests : BasePlanUseCaseTests
{
	[Fact]
	public async Task Test_ListAllPlansUseCase_PlansSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<ListAllPlansUseCase>();
		var command = new ListAllPlansCommand();

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.PlansSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<IEnumerable<PlanModel>>>(result).Data;
		Assert.Equal(_mock.Plans.Count, resultData.Count());

		_mock.PlanRepository.Verify(repo => repo.ListAllAsync(), Times.Once);
	}
}
