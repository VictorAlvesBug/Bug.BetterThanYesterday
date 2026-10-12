using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Plans.UpdatePlanPeriod;

public sealed class UpdatePlanPeriodUseCase(
	IPlanRepository planRepository,
	IPlanMemberRepository planMemberRepository,
	IHabitRepository habitRepository,
	IUserRepository userRepository)
	: IUseCase<UpdatePlanPeriodCommand>
{
	public async Task<IResult> HandleAsync(UpdatePlanPeriodCommand command)
	{
		try
		{
			command.Validate();

			var plan = await planRepository.GetByIdAsync(command.PlanId);

			if (plan is null)
				return Result.Rejected(Messages.PlanNotFound, RejectionType.NotFound);

			if (plan.OwnerId != command.OwnerId)
				return Result.Rejected(Messages.EnterOwnerId);

			var habit = await habitRepository.GetByIdAsync(plan.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

			var owner = await userRepository.GetByIdAsync(plan.OwnerId);

			if (owner is null)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

			var members = await planMemberRepository.ListByPlanIdAsync(plan.Id);
			plan.UpdatePeriod(
				DateOnly.FromDateTime(command.StartsAt),
				DateOnly.FromDateTime(command.EndsAt),
				members.Count);
			await planRepository.UpdateAsync(plan);

			return Result.Success(
				plan.ToModel(habit, owner, members.Count),
				Messages.PlanSuccessfullyRegistered);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
