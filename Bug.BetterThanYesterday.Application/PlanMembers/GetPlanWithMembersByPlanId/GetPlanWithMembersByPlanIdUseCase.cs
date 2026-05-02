using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.PlanMembers.GetPlanWithMembersByPlanId;

public sealed class GetPlanWithMembersByPlanIdUseCase(
    IPlanMemberRepository planMemberRepository,
    IPlanRepository planRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository)
    : IUseCase<GetPlanWithMembersByPlanIdCommand>
{
    public async Task<IResult> HandleAsync(GetPlanWithMembersByPlanIdCommand command)
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

            var planMembers = await planMemberRepository.ListByPlanIdAsync(command.PlanId);

            if (planMembers.Count == 0)
                return Result.Success(plan.ToPlanWithMembersModel(habit), Messages.PlanHasNoMembers);

            var memberIds = planMembers.Select(planMember => planMember.UserId).ToList();

            var members = await userRepository.BatchGetByIdAsync(memberIds);

            if (memberIds.Count > members.Count)
            {
                var notFoundIds = memberIds.Where(id => !members.Any(p => p.Id == id));
                var strNotFoundIds = string.Join(", ", notFoundIds);
                return Result.Rejected($"Usuários não encontrados para os IDs: {strNotFoundIds}");
            }

            return Result.Success(
                plan.ToPlanWithMembersModel(habit, members),
                Messages.PlanSuccessfullyFound
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}