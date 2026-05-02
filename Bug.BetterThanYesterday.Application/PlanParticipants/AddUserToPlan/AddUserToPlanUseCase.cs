using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.PlanParticipants;
using Bug.BetterThanYesterday.Domain.PlanParticipants.Entities;
using Bug.BetterThanYesterday.Domain.PlanParticipants.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Habits;

namespace Bug.BetterThanYesterday.Application.PlanParticipants.AddUserToPlan;

public sealed class AddUserToPlanUseCase(
    IPlanParticipantRepository planParticipantRepository,
    IPlanRepository planRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository)
    : IUseCase<AddUserToPlanCommand>
{
    public async Task<IResult> HandleAsync(AddUserToPlanCommand command)
    {
        try
        {
            command.Validate();

            var plan = await planRepository.GetByIdAsync(command.PlanId);

            if (plan is null)
                return Result.Rejected(Messages.PlanNotFound);

            var habit = await habitRepository.GetByIdAsync(plan.HabitId);

            if (habit is null)
                return Result.Rejected(Messages.HabitNotFound);

            var user = await userRepository.GetByIdAsync(command.UserId);

            if (user is null)
                return Result.Rejected(Messages.UserNotFound);

            if (plan.Status != PlanStatus.NotStarted)
                return Result.Rejected(Messages.OnlyNotStartedPlansCanReceiveNewParticipants);

            var planParticipantToAdd = PlanParticipant.CreateNew(command.PlanId, command.UserId);

            var planParticipantDetailsModel = planParticipantToAdd.ToPlanParticipantDetailsModel(habit, plan, user);

            var existingPlanParticipant = await planParticipantRepository.GetByIdAsync(planParticipantToAdd.Id);

            if (existingPlanParticipant is null)
            {
                await planParticipantRepository.AddAsync(planParticipantToAdd);
                return Result.Success(
                    planParticipantDetailsModel,
                    Messages.ParticipantSuccessfullyAddedToThePlan
                );
            }

            if (existingPlanParticipant.Status == PlanParticipantStatus.Left)
            {
                await planParticipantRepository.UpdateAsync(planParticipantToAdd);
                return Result.Success(
                    planParticipantDetailsModel,
                    Messages.ParticipantSuccessfullyReaddedToThePlan
                );
            }

            return Result.Rejected(Messages.ParticipantAlreadyAddedToThePlan);
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}