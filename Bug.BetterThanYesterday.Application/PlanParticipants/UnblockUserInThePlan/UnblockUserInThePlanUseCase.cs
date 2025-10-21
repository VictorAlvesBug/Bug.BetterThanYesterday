using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.PlanParticipants;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.UnblockUserInThePlan;

public sealed class UnblockUserInThePlanUseCase(
    IPlanParticipantRepository planParticipantRepository,
    IPlanRepository planRepository,
    IUserRepository userRepository)
    : IUseCase<UnblockUserInThePlanCommand>
{
    public async Task<IResult> HandleAsync(UnblockUserInThePlanCommand command)
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

        if (plan.Status != PlanStatus.Running)
            return Result.Rejected(Messages.ParticipantCanOnlyBeUnblockedInRunningPlans);

        try
        {
            planParticipant.MarkAsActive();
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
        
        await planParticipantRepository.UpdateAsync(planParticipant);
        return Result.Success(
            planParticipant.ToPlanParticipantDetailsModel(plan, user),
            Messages.ParticipantSuccessfullyUnblocked
		);
    }
}