using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.PlanParticipants;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.RemoveUserFromPlan;

public sealed class RemoveUserFromPlanUseCase(IPlanParticipantRepository planParticipantRepository)
    : IUseCase<RemoveUserFromPlanCommand>
{
    public async Task<IResult> HandleAsync(RemoveUserFromPlanCommand command)
    {
		command.Validate();
        await Task.Run(() => { });
        throw new NotImplementedException();
    }
}