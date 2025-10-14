using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.PlanParticipants;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.GetPlanParticipantDetails;

public sealed class GetPlanParticipantDetailsUseCase(
    IPlanParticipantRepository planParticipantRepository,
    IPlanRepository planRepository,
    IUserRepository userRepository)
    : IUseCase<GetPlanParticipantDetailsCommand>
{
    public async Task<IResult> HandleAsync(GetPlanParticipantDetailsCommand command)
    {
        command.Validate();
        var planParticipantId = PlanParticipant.GenerateId(
            command.PlanId,
            command.UserId
        );
        var planParticipant = await planParticipantRepository.GetByIdAsync(planParticipantId);

        if (planParticipant is null)
            return Result.Rejected("Participante do plano não encontrado");

        var plan = await planRepository.GetByIdAsync(planParticipant.PlanId);

        if (plan is null)
            return Result.Rejected("Plano não encontrado");

        var user = await userRepository.GetByIdAsync(planParticipant.UserId);

        if (user is null)
            return Result.Rejected("Usuário não encontrado");

        return Result.Success(planParticipant.ToPlanParticipantDetailsModel(plan, user));
    }
}
