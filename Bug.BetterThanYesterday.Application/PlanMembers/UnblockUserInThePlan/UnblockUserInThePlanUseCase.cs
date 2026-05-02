using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.PlanMembers.UnblockUserInThePlan;

public sealed class UnblockUserInThePlanUseCase(
    IPlanMemberRepository planMemberRepository,
    IPlanRepository planRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository)
    : IUseCase<UnblockUserInThePlanCommand>
{
    public async Task<IResult> HandleAsync(UnblockUserInThePlanCommand command)
    {
        try
        {
            command.Validate();
            
            var plan = await planRepository.GetByIdAsync(command.PlanId);

            if (plan is null)
                return Result.Rejected(Messages.PlanNotFound);

            var habit = await habitRepository.GetByIdAsync(plan.HabitId);

            if (habit is null)
                return Result.Rejected(Messages.HabitNotFound);

            var user = await userRepository.GetByIdAsync(command.UserId);

            if (user is null)
                return Result.Rejected(Messages.UserNotFound);

            var planMemberId = PlanMember.BuildId(command.PlanId, command.UserId);

            var planMember = await planMemberRepository.GetByIdAsync(planMemberId);

            if (planMember is null)
                return Result.Rejected(Messages.UserIsNotInThePlan);

            if (plan.Status != PlanStatus.Running)
                return Result.Rejected(Messages.MemberCanOnlyBeUnblockedInRunningPlans);

            planMember.MarkAsActive();

            await planMemberRepository.UpdateAsync(planMember);
            return Result.Success(
                planMember.ToPlanMemberDetailsModel(habit, plan, user),
                Messages.MemberSuccessfullyUnblocked
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}