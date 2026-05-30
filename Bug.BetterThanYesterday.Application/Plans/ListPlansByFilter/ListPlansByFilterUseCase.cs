using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Plans.ListPlansByFilter;

public class ListPlansByFilterUseCase(
	IUserRepository userRepository,
	IPlanRepository planRepository,
	IHabitRepository habitRepository)
	: IUseCase<ListPlansByFilterCommand>
{
	public async Task<IResult> HandleAsync(ListPlansByFilterCommand command)
	{
		try
		{
			command.Validate();

			List<Plan> plans = [];

			if (command.OwnerId is not null && command.OwnerId.HasValue)
			{
				var owner = await userRepository.GetByIdAsync(command.OwnerId.Value);

				if (owner is null)
					return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

				plans = await planRepository.ListByOwnerIdAsync(command.OwnerId.Value);
			}
			else
			{
				plans = await planRepository.ListAllAsync();
			}
			
			if (command.HabitId is not null && command.HabitId.HasValue)
			{
				var habit = await habitRepository.GetByIdAsync(command.HabitId.Value);

				if (habit is null)
					return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

				plans = plans.Where(plan => plan.HabitId == command.HabitId.Value).ToList();
				//plans = await planRepository.ListByHabitIdAsync(command.HabitId.Value);
			}

			if (!string.IsNullOrEmpty(command.Status))
			{
				if (!PlanStatus.TryGet(command.Status, out var status, out var errorMessage))
					return Result.Rejected(errorMessage ?? Messages.GenericError);

				plans = plans
					.Where(plan => plan.GetStatus() == status)
					.ToList();
			}

			if (!string.IsNullOrEmpty(command.Type))
			{
				if (!PlanType.TryGet(command.Type, out var type, out var errorMessage))
					return Result.Rejected(errorMessage ?? Messages.GenericError);

				plans = plans
					.Where(plan => plan.Type == type)
					.ToList();
			}

			var ownerIds = plans.Select(plan => plan.OwnerId).Distinct().ToList();
			var owners = ownerIds.Count == 0
				? []
				: await userRepository.BatchGetByIdAsync(ownerIds);

			if (ownerIds.Count > owners.Count)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

			var ownersById = owners.ToDictionary(owner => owner.Id);

			var tasks = plans.Select(async plan =>
			{
				var habit = await habitRepository.GetByIdAsync(plan.HabitId) ?? throw new Exception(Messages.HabitNotFound);
				return plan.ToModel(habit, ownersById[plan.OwnerId]);
			}).ToList();

			return Result.Success(
				await Task.WhenAll(tasks)
			);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
