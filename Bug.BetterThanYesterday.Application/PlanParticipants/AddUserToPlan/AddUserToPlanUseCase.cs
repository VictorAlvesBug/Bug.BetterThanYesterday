using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.PlanParticipants;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.PlanParticipants.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.AddUserToPlan;

public sealed class AddUserToPlanUseCase(
    IPlanParticipantRepository planParticipantRepository,
    IPlanRepository planRepository,
    IUserRepository userRepository)
    : IUseCase<AddUserToPlanCommand>
{
    public async Task<IResult> HandleAsync(AddUserToPlanCommand command)
    {
        command.Validate();

        var plan = await planRepository.GetByIdAsync(command.PlanId);

        if (plan is null)
            return Result.Rejected("Plano não encontrado");

        var user = await userRepository.GetByIdAsync(command.UserId);

        if (user is null)
            return Result.Rejected("Usuário não encontrado");

        if (plan.Status != PlanStatus.NotStarted)
            return Result.Rejected("Apenas planos não iniciados podem receber novos participantes");

        var planParticipantToAdd = PlanParticipant.CreateNew(command.PlanId, command.UserId);

        var planParticipantDetailsModel = planParticipantToAdd.ToPlanParticipantDetailsModel(plan, user);

        var existingPlanParticipant = await planParticipantRepository.GetByIdAsync(planParticipantToAdd.Id);

        if (existingPlanParticipant is null)
        {
            await planParticipantRepository.AddAsync(planParticipantToAdd);
            return Result.Success(
                planParticipantDetailsModel,
                "Participante adicionado ao plano com sucesso"
            );
        }

        if (existingPlanParticipant.Status == PlanParticipantStatus.Left)
        {
            await planParticipantRepository.UpdateAsync(planParticipantToAdd);
            return Result.Success(
                planParticipantDetailsModel,
                "Participante readicionado ao plano com sucesso"
            );
        }
        
        return Result.Rejected("Participante já adicionado ao plano");
    }
}