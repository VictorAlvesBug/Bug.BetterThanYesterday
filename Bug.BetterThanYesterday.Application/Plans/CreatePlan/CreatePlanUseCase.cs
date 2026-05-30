using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Plans.CreatePlan;

public class CreatePlanUseCase(
	IUserRepository userRepository,
	IPlanRepository planRepository,
	IHabitRepository habitRepository)
	: IUseCase<CreatePlanCommand>
{
	public async Task<IResult> HandleAsync(CreatePlanCommand command)
	{
		try
		{
			command.Validate();
			
			var owner = await userRepository.GetByIdAsync(command.OwnerId);

			if (owner is null)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);
			
			var habit = await habitRepository.GetByIdAsync(command.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

			var plan = Plan.CreateNew(
				command.OwnerId,
				command.HabitId,
				command.Description,
				command.StartsAt,
				command.EndsAt,
				command.Type,
				command.DaysOffPerWeek,
				command.PenaltyValue);

			await planRepository.AddAsync(plan);
			return Result.Success(plan.ToModel(habit, owner), Messages.PlanSuccessfullyRegistered);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
