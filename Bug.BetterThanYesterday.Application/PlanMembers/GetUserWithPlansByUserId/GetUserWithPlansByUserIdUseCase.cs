using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Ledger;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Penalties.ValueObjects;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.PlanMembers.GetUserWithPlansByUserId;

public sealed class GetUserWithPlansByUserIdUseCase(
    IPlanMemberRepository planMemberRepository,
    IPlanRepository planRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository,
    IPenaltyRepository penaltyRepository)
    : IUseCase<GetUserWithPlansByUserIdCommand>
{
    public async Task<IResult> HandleAsync(GetUserWithPlansByUserIdCommand command)
    {
        try
        {
            command.Validate();
            
            var user = await userRepository.GetByIdAsync(command.UserId);

            if (user is null)
                return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

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

            var ownerIds = plans.Select(plan => plan.OwnerId).Distinct().ToList();
            var owners = ownerIds.Count == 0
                ? []
                : await userRepository.BatchGetByIdAsync(ownerIds);

            if (ownerIds.Count > owners.Count)
                return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

            var ownersById = owners.ToDictionary(owner => owner.Id);

            var tasks = plans.Select(async plan => {
                var habit = await habitRepository.GetByIdAsync(plan.HabitId) ?? throw new Exception(Messages.HabitNotFound);
                var model = plan.ToModel(habit, ownersById[plan.OwnerId]);
                var penalties = await penaltyRepository.ListByPlanIdAsync(plan.Id);
                model.AmountOwed = penalties
                    .Where(penalty => penalty.UserId == command.UserId && penalty.Status == PenaltyStatus.Pending)
                    .Sum(penalty => penalty.Amount);
                var paidPot = penalties
                    .Where(penalty => penalty.Status == PenaltyStatus.Paid)
                    .Sum(penalty => penalty.Amount);
                model.RewardPool = SettlementCalculator.DistributableAmount(paidPot);
                return model;
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