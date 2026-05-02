using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.PlanParticipants;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.GetPlanParticipantDetails;

public sealed class GetPlanParticipantDetailsUseCase(
    IPlanParticipantRepository planParticipantRepository,
    IPlanRepository planRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository)
    : IUseCase<GetPlanParticipantDetailsCommand>
{
    public async Task<IResult> HandleAsync(GetPlanParticipantDetailsCommand command)
    {
        try
        {
            command.Validate();
            
            var planParticipantId = PlanParticipant.BuildId(
                command.PlanId,
                command.UserId
            );
            var planParticipant = await planParticipantRepository.GetByIdAsync(planParticipantId);

            if (planParticipant is null)
                return Result.Rejected(Messages.PlanParticipantNotFound);

            var plan = await planRepository.GetByIdAsync(planParticipant.PlanId);

            if (plan is null)
                return Result.Rejected(Messages.PlanNotFound);

            var habit = await habitRepository.GetByIdAsync(plan.HabitId);

            if (habit is null)
                return Result.Rejected(Messages.HabitNotFound);

            var user = await userRepository.GetByIdAsync(planParticipant.UserId);

            if (user is null)
                return Result.Rejected(Messages.UserNotFound);

            return Result.Success(
                planParticipant.ToPlanParticipantDetailsModel(habit, plan, user),
                Messages.PlanParticipantSuccessfullyFound
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}
