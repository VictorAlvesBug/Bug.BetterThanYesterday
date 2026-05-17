using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Habits;

namespace Bug.BetterThanYesterday.Application.PlanMembers.AddUserToPlan;

public sealed class AddUserToPlanUseCase(
    IPlanMemberRepository planMemberRepository,
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

            var owner = plan.OwnerId == user.Id
                ? user
                : (await userRepository.BatchGetByIdAsync([plan.OwnerId])).SingleOrDefault();

            if (owner is null)
                return Result.Rejected(Messages.UserNotFound);

            if (plan.GetStatus() != PlanStatus.NotStarted)
                return Result.Rejected(Messages.OnlyNotStartedPlansCanReceiveNewMembers);

            var planMemberToAdd = PlanMember.CreateNew(command.PlanId, command.UserId);

            var planMemberDetailsModel = planMemberToAdd.ToPlanMemberDetailsModel(habit, plan, owner, user);

            var existingPlanMember = await planMemberRepository.GetByIdAsync(planMemberToAdd.Id);

            if (existingPlanMember is null)
            {
                await planMemberRepository.AddAsync(planMemberToAdd);
                return Result.Success(
                    planMemberDetailsModel,
                    Messages.MemberSuccessfullyAddedToThePlan
                );
            }

            return Result.Rejected(Messages.MemberAlreadyAddedToThePlan);
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}