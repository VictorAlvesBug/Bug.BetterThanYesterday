using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.ReplaceCheckIn;

public sealed class ReplaceCheckInUseCase(
	ICheckInRepository checkInRepository,
	IPlanRepository planRepository,
	IHabitRepository habitRepository,
	IUserRepository userRepository)
	: IUseCase<ReplaceCheckInCommand>
{
	public async Task<IResult> HandleAsync(ReplaceCheckInCommand command)
	{
		try
		{
			command.Validate();

			var checkIn = await checkInRepository.GetByIdAsync(command.CheckInId);

			if (checkIn is null)
				return Result.Rejected(Messages.CheckInNotFound, RejectionType.NotFound);

			if (checkIn.UserId != command.UserId)
				return Result.Rejected(Messages.OnlyCheckInOwnerCanReplace);

			var plan = await planRepository.GetByIdAsync(checkIn.PlanId);

			if (plan is null)
				return Result.Rejected(Messages.PlanNotFound, RejectionType.NotFound);

			var habit = await habitRepository.GetByIdAsync(plan.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

			var user = await userRepository.GetByIdAsync(checkIn.UserId);

			if (user is null)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

			checkIn.ReplaceEvidence(command.Title, command.EvidencePhotoUrl);
			await checkInRepository.UpdateAsync(checkIn);

			return Result.Success(
				checkIn.ToModel(plan, habit, user),
				Messages.CheckInSuccessfullyRegistered);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
