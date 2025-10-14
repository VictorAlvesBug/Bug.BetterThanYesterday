using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.PlanParticipants;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.GetPlanWithParticipantsByPlanId;

public sealed class GetPlanWithParticipantsByPlanIdUseCase(
    IPlanParticipantRepository planParticipantRepository,
    IPlanRepository planRepository,
    IUserRepository userRepository)
    : IUseCase<GetPlanWithParticipantsByPlanIdCommand>
{
    public async Task<IResult> HandleAsync(GetPlanWithParticipantsByPlanIdCommand command)
    {
		command.Validate();

        var plan = await planRepository.GetByIdAsync(command.PlanId);

        if (plan is null)
            return Result.Rejected("Plano não encontrado");

        var planParticipants = await planParticipantRepository.ListByPlanIdAsync(command.PlanId);

        if (planParticipants.Count == 0)
            return Result.Success(plan.ToPlanWithParticipantsModel(), "Nenhum participante encontrado para este plano");

        var participantIds = planParticipants.Select(planParticipant => planParticipant.UserId).ToList();

        var participants = await userRepository.BatchGetByIdAsync(participantIds);

        if (participantIds.Count > participants.Count)
        {
            var notFoundIds = participantIds.Where(id => !participants.Any(p => p.Id == id));
            var strNotFoundIds = string.Join(", ", notFoundIds);
            return Result.Rejected($"Usuários não encontrados para os IDs: {strNotFoundIds}");
        }

        return Result.Success(plan.ToPlanWithParticipantsModel(participants));
    }
}