using Bug.BetterThanYesterday.Application.PlanMembers;
using Bug.BetterThanYesterday.Application.PlanMembers.GetPlanMemberDetails;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Moq;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.UseCases.PlanMemberUseCases;

public class GetPlanMemberDetailsUseCaseTests : BasePlanMemberUseCaseTests
{
	[Fact]
	public async Task Test_GetPlanMemberDetailsUseCase_PlanMemberSuccessfullyFound_ShouldReturnSuccess()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetPlanMemberDetailsUseCase>();
		var firstPlanMember = _mock.PlanMembers[0];
		var plan = _mock.Plans.First(plan => plan.Id == firstPlanMember.PlanId);
		var member = _mock.Users.First(user => user.Id == firstPlanMember.UserId);
		var command = new GetPlanMemberDetailsCommand(
			firstPlanMember.PlanId,
			firstPlanMember.UserId
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsSuccess());
		Assert.Equal(Messages.PlanMemberSuccessfullyFound, result.GetMessage());

		var resultData = Assert.IsType<Result<PlanMemberDetailsModel>>(result).Data;
		Assert.Equal(firstPlanMember.Id, resultData.PlanMemberId);
		Assert.Equal(firstPlanMember.JoinedAt.ToDateTime(TimeOnly.MinValue), resultData.JoinedAt);
		Assert.Equal(firstPlanMember.LeftAt?.ToDateTime(TimeOnly.MinValue), resultData.LeftAt);
		Assert.Equal(firstPlanMember.Status.Id, resultData.StatusId);
		Assert.Equal(firstPlanMember.Status.Name, resultData.StatusName);
		
		Assert.Equal(plan.Id, resultData.Plan.PlanId);
		Assert.Equal(plan.OwnerId, resultData.Plan.OwnerId);
		Assert.Equal(plan.HabitId, resultData.Plan.HabitId);
		Assert.Equal(plan.Description, resultData.Plan.Description);
		Assert.Equal(plan.StartsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.StartsAt);
		Assert.Equal(plan.EndsAt.ToDateTime(TimeOnly.MinValue), resultData.Plan.EndsAt);
		Assert.Equal(plan.GetStatus(), PlanStatus.Get(resultData.Plan.Status));
		Assert.Equal(plan.Type, PlanType.Get(resultData.Plan.Type));
		Assert.Equal(plan.DaysOffPerWeek, resultData.Plan.DaysOffPerWeek);
		Assert.Equal(plan.PenaltyValue, resultData.Plan.PenaltyValue);
		Assert.Equal(plan.CreatedAt, resultData.Plan.CreatedAt);
		
		Assert.Equal(member.Id, resultData.Member.UserId);
		Assert.Equal(member.Name, resultData.Member.Name);
		Assert.Equal(member.Email.Value, resultData.Member.Email);
		Assert.Equal(member.CreatedAt, resultData.Member.CreatedAt);

		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
	}

	[Fact]
	public async Task Test_GetPlanMemberDetailsUseCase_PlanMemberNotFound_ShouldReturnRejected()
	{
		// Arrange
		var useCase = _mocker.CreateInstance<GetPlanMemberDetailsUseCase>();
		var firstPlanMember = _mock.PlanMembers[0];
		var command = new GetPlanMemberDetailsCommand(
			Guid.NewGuid(),
			Guid.NewGuid()
		);

		// Act
		var result = await useCase.HandleAsync(command);

		// Assert
		Assert.NotNull(result);
		Assert.True(result.IsRejected());
		Assert.Equal(Messages.PlanMemberNotFound, result.GetMessage());

		_mock.PlanMemberRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
		_mock.PlanRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
		_mock.UserRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
	}
}
