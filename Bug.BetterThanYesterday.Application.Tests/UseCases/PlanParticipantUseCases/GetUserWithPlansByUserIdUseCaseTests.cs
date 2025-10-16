using Bug.BetterThanYesterday.Application.PlanParticipants;
using Bug.BetterThanYesterday.Application.PlanParticipants.GetUserWithPlansByUserId;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Tests.Commons;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanParticipantUseCases;

public class GetUserWithPlansByUserIdUseCaseTests : BasePlanParticipantUseCaseTests
{
	[Fact]
	public async Task Test_GetUserWithPlansByUserIdUseCase_Valid_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetUserWithPlansByUserIdUseCase>();
		var userId = _mock.PlanParticipants.First().UserId;
		var user = _mock.Users.First(user => user.Id == userId);
		var plans =
			(
				from
					planParticipant in _mock.PlanParticipants
				where
					planParticipant.UserId == userId
				join plan in _mock.Plans 
					on planParticipant.PlanId equals plan.Id
				select
					plan
			).Distinct()
			.ToList();
		var command = new GetUserWithPlansByUserIdCommand(userId);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

		var resultData = Assert.IsType<Result<UserWithPlansModel>>(result).Data;
		Assert.Equal(user.Id, resultData.User.UserId);
		Assert.Equal(user.Name, resultData.User.Name);
		Assert.Equal(user.Email.Value, resultData.User.Email);
		Assert.Equal(user.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.User.CreatedAt);
		
		Assert.Equal(plans.Count, resultData.Plans.Count);

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.ListByUserIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Once);
	}

	[Fact]
	public async Task Test_GetUserWithPlansByUserIdUseCase_UserWithoutPlans_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetUserWithPlansByUserIdUseCase>();
		var user = _mock.Users.Find(user => user.Id == UserRepositoryMockFactory.UserId4);
		var command = new GetUserWithPlansByUserIdCommand(user.Id);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());

		var resultData = Assert.IsType<Result<UserWithPlansModel>>(result).Data;
		Assert.Equal(user.Id, resultData.User.UserId);
		Assert.Equal(user.Name, resultData.User.Name);
		Assert.Equal(user.Email.Value, resultData.User.Email);
		Assert.Equal(user.CreatedAt.ToDateTime(TimeOnly.MinValue), resultData.User.CreatedAt);
		
		Assert.Empty(resultData.Plans);

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.ListByUserIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Never);
	}

	[Fact]
	public async Task Test_GetUserWithPlansByUserIdUseCase_NotFoundUserId_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetUserWithPlansByUserIdUseCase>();
		var command = new GetUserWithPlansByUserIdCommand(Guid.NewGuid());

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());

		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanParticipantRepository.Verify(repo => repo.ListByUserIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.PlanRepository.Verify(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()), Times.Never);
	}
}
