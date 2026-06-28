using Bug.BetterThanYesterday.Application.DayOffs.Models;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.DayOffs;
using Bug.BetterThanYesterday.Domain.DayOffs.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.DayOffs.UseDayOff;

public sealed class UseDayOffUseCase(
	IDayOffRepository dayOffRepository,
	ICheckInRepository checkInRepository,
	IPlanRepository planRepository,
	IPlanMemberRepository planMemberRepository)
	: IUseCase<UseDayOffCommand>
{
	public async Task<IResult> HandleAsync(UseDayOffCommand command)
	{
		try
		{
			command.Validate();

			var plan = await planRepository.GetByIdAsync(command.PlanId);

			if (plan is null)
				return Result.Rejected(Messages.PlanNotFound, RejectionType.NotFound);

			if (plan.GetStatus() != PlanStatus.Running)
				return Result.Rejected(Messages.OnlyRunningPlansCanReceiveDayOffs);

			var planMemberId = PlanMember.BuildId(command.PlanId, command.UserId);
			var planMember = await planMemberRepository.GetByIdAsync(planMemberId);

			if (planMember is null)
				return Result.Rejected(Messages.PlanMemberNotFound, RejectionType.NotFound);

			if (planMember.Status != PlanMemberStatus.Active)
				return Result.Rejected(Messages.OnlyActiveMembersCanMakeCheckIns);

			var dateOnly = DateOnly.FromDateTime(command.Date);
			var today = DateOnly.FromDateTime(DateTime.Today);

			if (dateOnly > today)
				return Result.Rejected(Messages.DayOffDateCannotBeInTheFuture);

			if (dateOnly < plan.StartsAt || dateOnly > plan.EndsAt)
				return Result.Rejected(Messages.DayOffDateOutsidePlanRange);

			var existingDayOff = await dayOffRepository.GetByPlanIdAndUserIdAndDateAsync(
				command.PlanId,
				command.UserId,
				dateOnly);

			if (existingDayOff is not null)
				return Result.Rejected(Messages.DayOffAlreadyExistsForDate);

			var checkInsOnDate = await checkInRepository.ListByPlanIdAndUserIdAndDateAsync(
				command.PlanId,
				command.UserId,
				dateOnly);

			if (checkInsOnDate.Count > 0)
				return Result.Rejected(Messages.CheckInAlreadyExistsForDate);

			var userDayOffs = await dayOffRepository.ListByPlanIdAndUserIdAsync(
				command.PlanId,
				command.UserId);

			var daysOffAvailable = DayOffMapper.GetDaysOffAvailable(plan, userDayOffs, today);

			if (daysOffAvailable <= 0)
				return Result.Rejected(Messages.NoDayOffAvailable);

			var dayOff = DayOff.CreateNew(command.PlanId, command.UserId, command.Date);
			await dayOffRepository.AddAsync(dayOff);

			var remaining = DayOffMapper.GetDaysOffAvailable(
				plan,
				userDayOffs.Append(dayOff),
				today);

			return Result.Success(
				new UseDayOffResultModel
				{
					DayOff = dayOff.ToModel(),
					DaysOffAvailable = remaining
				},
				Messages.DayOffSuccessfullyRegistered);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
