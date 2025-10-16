using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.PlanParticipants;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.UnblockUserInThePlan;

public sealed class UnblockUserInThePlanUseCase(
    IPlanParticipantRepository planParticipantRepository,
    IPlanRepository planRepository,
    IUserRepository userRepository)
    : IUseCase<UnblockUserInThePlanCommand>
{
    public async Task<IResult> HandleAsync(UnblockUserInThePlanCommand command)
    {
        command.Validate();

        var plan = await planRepository.GetByIdAsync(command.PlanId);

        if (plan is null)
            return Result.Rejected("Plano não encontrado");

        var user = await userRepository.GetByIdAsync(command.UserId);

        if (user is null)
            return Result.Rejected("Usuário não encontrado");

        var planParticipantId = PlanParticipant.BuildId(command.PlanId, command.UserId);

        var planParticipant = await planParticipantRepository.GetByIdAsync(planParticipantId);

        if (planParticipant is null)
            return Result.Rejected("Usuário não está neste plano");

        if (plan.Status != PlanStatus.Running)
            return Result.Rejected("O status atual do plano não permite o desbloqueio desse participante");

        planParticipant.MarkAsActive();
        
        await planParticipantRepository.UpdateAsync(planParticipant);
        return Result.Success(
            planParticipant.ToPlanParticipantDetailsModel(plan, user),
            "Participante bloqueado com sucesso"
        );
    }
}