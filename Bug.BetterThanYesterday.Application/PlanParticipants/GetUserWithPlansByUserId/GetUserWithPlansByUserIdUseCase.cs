using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.PlanParticipants;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.GetUserWithPlansByUserId;

public sealed class GetUserWithPlansByUserIdUseCase(
    IPlanParticipantRepository planParticipantRepository,
    IPlanRepository planRepository,
    IUserRepository userRepository)
    : IUseCase<GetUserWithPlansByUserIdCommand>
{
    public async Task<IResult> HandleAsync(GetUserWithPlansByUserIdCommand command)
    {
		command.Validate();

        var user = await userRepository.GetByIdAsync(command.UserId);

        if (user is null)
            return Result.Rejected(Messages.UserNotFound);

        var planParticipants = await planParticipantRepository.ListByUserIdAsync(command.UserId);

        if (planParticipants.Count == 0)
            return Result.Success(user.ToUserWithPlansModel(), Messages.UserHasNoPlans);

        var planIds = planParticipants.Select(planParticipant => planParticipant.PlanId).ToList();

        var plans = await planRepository.BatchGetByIdAsync(planIds);

        if (planIds.Count > plans.Count)
        {
            var notFoundIds = planIds.Where(id => !plans.Any(p => p.Id == id));
            var strNotFoundIds = string.Join(", ", notFoundIds);
            return Result.Rejected($"Planos não encontrados para os IDs: {strNotFoundIds}");
        }

        return Result.Success(
            user.ToUserWithPlansModel(plans),
            Messages.UserSuccessfullyFound);
    }
}