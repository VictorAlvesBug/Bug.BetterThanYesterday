using Bug.BetterThanYesterday.Application.DayOffs;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.DayOffs;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.DayOffs.GetDayOffAvailability;

public sealed class GetDayOffAvailabilityUseCase(
	IDayOffRepository dayOffRepository,
	IPlanRepository planRepository,
	IPlanMemberRepository planMemberRepository)
	: IUseCase<GetDayOffAvailabilityCommand>
{
	public async Task<IResult> HandleAsync(GetDayOffAvailabilityCommand command)
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

			var userDayOffs = await dayOffRepository.ListByPlanIdAndUserIdAsync(
				command.PlanId,
				command.UserId);

			var today = DateOnly.FromDateTime(DateTime.Today);
			var daysOffAvailable = DayOffMapper.GetDaysOffAvailable(plan, userDayOffs, today);

			return Result.Success(daysOffAvailable, Messages.PlanSuccessfullyFound);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
