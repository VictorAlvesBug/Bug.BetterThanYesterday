using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.PlanParticipants;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.AddUserToPlan;

public sealed class AddUserToPlanUseCase(IPlanParticipantRepository planParticipantRepository)
    : IUseCase<AddUserToPlanCommand>
{
    public async Task<IResult> HandleAsync(AddUserToPlanCommand command)
    {
		command.Validate();
        await Task.Run(() => { });
        throw new NotImplementedException();
    }
}