using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Penalties.ListPenaltiesByPlanMember;

public sealed class ListPenaltiesByPlanMemberUseCase(
	IPenaltyRepository penaltyRepository,
	IPlanRepository planRepository,
	IPlanMemberRepository planMemberRepository)
	: IUseCase<ListPenaltiesByPlanMemberCommand>
{
	public async Task<IResult> HandleAsync(ListPenaltiesByPlanMemberCommand command)
	{
		try
		{
			command.Validate();

			var plan = await planRepository.GetByIdAsync(command.PlanId);

			if (plan is null)
				return Result.Rejected(Messages.PlanNotFound, RejectionType.NotFound);

			var planMemberId = PlanMember.BuildId(command.PlanId, command.UserId);
			var planMember = await planMemberRepository.GetByIdAsync(planMemberId);

			if (planMember is null)
				return Result.Rejected(Messages.PlanMemberNotFound, RejectionType.NotFound);

			var penalties = await penaltyRepository.ListByPlanIdAndUserIdAsync(command.PlanId, command.UserId);

			return Result.Success(
				penalties.Select(PenaltyMapper.ToModel).ToList(),
				Messages.PenaltiesSuccessfullyFound);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
