using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.PlanParticipants;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.BlockUserInThePlan;

public sealed class BlockUserInThePlanUseCase(IPlanParticipantRepository planParticipantRepository)
    : IUseCase<BlockUserInThePlanCommand>
{
    public async Task<IResult> HandleAsync(BlockUserInThePlanCommand command)
    {
        command.Validate();
        await Task.Run(() => { });
        throw new NotImplementedException();
    }
}