using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetCheckInDetails;

public sealed class GetCheckInDetailsUseCase(
    ICheckInRepository checkInRepository,
    IPlanRepository planRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository)
    : IUseCase<GetCheckInDetailsCommand>
{
    public async Task<IResult> HandleAsync(GetCheckInDetailsCommand command)
    {
        try
        {
            command.Validate();

            var plan = await planRepository.GetByIdAsync(command.PlanId);
            if (plan is null)
                return Result.Rejected(Messages.PlanNotFound, RejectionType.NotFound);

            var habit = await habitRepository.GetByIdAsync(plan.HabitId);
            if (habit is null)
                return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

            var user = await userRepository.GetByIdAsync(command.UserId);
            if (user is null)
                return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

            var checkIn = await checkInRepository.GetDetailsAsync(
                command.PlanId,
                command.UserId,
                DateOnly.FromDateTime(command.Date),
                command.Index
            );

            if (checkIn is null)
                return Result.Rejected(Messages.CheckInNotFound, RejectionType.NotFound);

            return Result.Success(
                checkIn.ToModel(plan, habit, user),
                Messages.CheckInSuccessfullyFound
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}