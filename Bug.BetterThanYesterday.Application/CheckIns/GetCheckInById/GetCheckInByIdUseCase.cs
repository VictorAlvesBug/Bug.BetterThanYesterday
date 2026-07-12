using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetCheckInById;

public sealed class GetCheckInByIdUseCase(
    ICheckInRepository checkInRepository,
    IPlanRepository planRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository,
    IPlanMemberRepository planMemberRepository)
    : IUseCase<GetCheckInByIdCommand>
{
    public async Task<IResult> HandleAsync(GetCheckInByIdCommand command)
	{
		try
		{
			command.Validate();
			
			var checkIn = await checkInRepository.GetByIdAsync(command.CheckInId);

			if (checkIn is null)
				return Result.Rejected(Messages.CheckInNotFound, RejectionType.NotFound);

			await CheckInStatusResolver.ConsolidateClosedWindowCheckInsAsync([checkIn], checkInRepository);

			var plan = await planRepository.GetByIdAsync(checkIn.PlanId);

			if (plan is null)
				return Result.Rejected(Messages.PlanNotFound, RejectionType.NotFound);

			var habit = await habitRepository.GetByIdAsync(plan.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

			var user = await userRepository.GetByIdAsync(checkIn.UserId);

			if (user is null)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

			return Result.Success(
                await checkIn.ToModelAsync(
					planRepository,
					habitRepository,
					userRepository
				)
            );
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}