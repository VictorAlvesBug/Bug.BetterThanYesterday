using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

using Bug.BetterThanYesterday.Domain.PlanMembers;

namespace Bug.BetterThanYesterday.Application.Plans.GetPlanById;

public class GetPlanByIdUseCase(
	IPlanRepository planRepository,
	IHabitRepository habitRepository,
	IUserRepository userRepository,
	IPlanMemberRepository planMemberRepository)
	: IUseCase<GetPlanByIdCommand>
{
	public async Task<IResult> HandleAsync(GetPlanByIdCommand command)
	{
		try
		{
			command.Validate();
			
			var plan = await planRepository.GetByIdAsync(command.PlanId);

			if (plan is null)
				return Result.Rejected(Messages.PlanNotFound, RejectionType.NotFound);

			var habit = await habitRepository.GetByIdAsync(plan.HabitId);

			if (habit is null)
				return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

			var owner = await userRepository.GetByIdAsync(plan.OwnerId);

			if (owner is null)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

			var members = await planMemberRepository.ListByPlanIdAsync(plan.Id);
			await PlanQuorumPostponer.PostponeWhileUnderQuorumAsync(plan, members.Count, planRepository);

			return Result.Success(
				plan.ToModel(habit, owner, members.Count),
				Messages.PlanSuccessfullyFound
			);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
