using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.PlanParticipants;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.RemoveUserFromPlan;

public sealed class RemoveUserFromPlanUseCase(
    IPlanParticipantRepository planParticipantRepository,
    IPlanRepository planRepository,
    IUserRepository userRepository)
    : IUseCase<RemoveUserFromPlanCommand>
{
    public async Task<IResult> HandleAsync(RemoveUserFromPlanCommand command)
    {
        try
        {
            command.Validate();
            
            var plan = await planRepository.GetByIdAsync(command.PlanId);

            if (plan is null)
                return Result.Rejected(Messages.PlanNotFound);

            var user = await userRepository.GetByIdAsync(command.UserId);

            if (user is null)
                return Result.Rejected(Messages.UserNotFound);

            var planParticipantId = PlanParticipant.BuildId(command.PlanId, command.UserId);

            var planParticipant = await planParticipantRepository.GetByIdAsync(planParticipantId);

            if (planParticipant is null)
                return Result.Rejected(Messages.UserIsNotInThePlan);

            var allowedPlanStatuses = new List<PlanStatus> { PlanStatus.NotStarted, PlanStatus.Running };

            if (!allowedPlanStatuses.Contains(plan.Status))
                return Result.Rejected(Messages.ParticipantCanOnlyBeRemovedFromNotStartedOrRunningPlans);

            planParticipant.MarkAsLeft();

            await planParticipantRepository.UpdateAsync(planParticipant);
            return Result.Success(
                planParticipant.ToPlanParticipantDetailsModel(plan, user),
                Messages.ParticipantSuccessfullyRemovedFromThePlan
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}