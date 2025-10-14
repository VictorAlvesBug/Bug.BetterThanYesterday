using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.PlanParticipants;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.UnblockUserInThePlan;

public sealed class UnblockUserInThePlanUseCase(IPlanParticipantRepository planParticipantRepository)
    : IUseCase<UnblockUserInThePlanCommand>
{
    public async Task<IResult> HandleAsync(UnblockUserInThePlanCommand command)
    {
		command.Validate();
        await Task.Run(() => { });
        throw new NotImplementedException();
    }
}