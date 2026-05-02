using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.PlanMembers.GetPlanMemberDetails;

public sealed class GetPlanMemberDetailsUseCase(
    IPlanMemberRepository planMemberRepository,
    IPlanRepository planRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository)
    : IUseCase<GetPlanMemberDetailsCommand>
{
    public async Task<IResult> HandleAsync(GetPlanMemberDetailsCommand command)
    {
        try
        {
            command.Validate();
            
            var planMemberId = PlanMember.BuildId(
                command.PlanId,
                command.UserId
            );
            var planMember = await planMemberRepository.GetByIdAsync(planMemberId);

            if (planMember is null)
                return Result.Rejected(Messages.PlanMemberNotFound);

            var plan = await planRepository.GetByIdAsync(planMember.PlanId);

            if (plan is null)
                return Result.Rejected(Messages.PlanNotFound);

            var habit = await habitRepository.GetByIdAsync(plan.HabitId);

            if (habit is null)
                return Result.Rejected(Messages.HabitNotFound);

            var user = await userRepository.GetByIdAsync(planMember.UserId);

            if (user is null)
                return Result.Rejected(Messages.UserNotFound);

            return Result.Success(
                planMember.ToPlanMemberDetailsModel(habit, plan, user),
                Messages.PlanMemberSuccessfullyFound
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}
