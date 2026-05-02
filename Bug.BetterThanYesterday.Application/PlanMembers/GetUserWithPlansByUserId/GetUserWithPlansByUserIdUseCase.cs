using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.PlanMembers.GetUserWithPlansByUserId;

public sealed class GetUserWithPlansByUserIdUseCase(
    IPlanMemberRepository planMemberRepository,
    IPlanRepository planRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository)
    : IUseCase<GetUserWithPlansByUserIdCommand>
{
    public async Task<IResult> HandleAsync(GetUserWithPlansByUserIdCommand command)
    {
        try
        {
            command.Validate();
            
            var user = await userRepository.GetByIdAsync(command.UserId);

            if (user is null)
                return Result.Rejected(Messages.UserNotFound);

            var planMembers = await planMemberRepository.ListByUserIdAsync(command.UserId);

            if (planMembers.Count == 0)
                return Result.Success(user.ToUserWithPlansModel(), Messages.UserHasNoPlans);

            var planIds = planMembers.Select(planMember => planMember.PlanId).ToList();

            var plans = await planRepository.BatchGetByIdAsync(planIds);

            if (planIds.Count > plans.Count)
            {
                var notFoundIds = planIds.Where(id => !plans.Any(p => p.Id == id));
                var strNotFoundIds = string.Join(", ", notFoundIds);
                return Result.Rejected($"Planos não encontrados para os IDs: {strNotFoundIds}");
            }

            var tasks = plans.Select(async plan => {
                var habit = await habitRepository.GetByIdAsync(plan.HabitId) ?? throw new Exception(Messages.HabitNotFound);
                return plan.ToModel(habit.ToModel());
            }).ToList();

            var planModels = (await Task.WhenAll(tasks)).ToList();

            return Result.Success(
                user.ToUserWithPlansModel(planModels),
                Messages.UserSuccessfullyFound);
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}