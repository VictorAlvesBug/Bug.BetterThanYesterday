using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetPlanWithCheckInsByPlanId;

public sealed class GetPlanWithCheckInsByPlanIdUseCase(
    ICheckInRepository checkInRepository,
    IPlanRepository planRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository)
    : IUseCase<GetPlanWithCheckInsByPlanIdCommand>
{
    public async Task<IResult> HandleAsync(GetPlanWithCheckInsByPlanIdCommand command)
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

            var owner = await userRepository.GetByIdAsync(plan.OwnerId);

            if (owner is null)
                return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

            var checkIns = await checkInRepository.ListByPlanIdAsync(command.PlanId);
            var userIds = checkIns.Select(checkIn => checkIn.UserId).Distinct().ToList();
            var users = userIds.Count == 0
                ? []
                : await userRepository.BatchGetByIdAsync(userIds);

            return Result.Success(
                plan.ToPlanWithCheckInsModel(habit, owner, checkIns, users),
                Messages.CheckInSuccessfullyFound
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}