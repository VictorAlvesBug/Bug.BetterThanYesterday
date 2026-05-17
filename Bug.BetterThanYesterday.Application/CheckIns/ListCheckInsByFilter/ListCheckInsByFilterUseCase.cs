using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.Extensions;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.PlanMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.CheckIns.ListCheckInsByFilter;

public sealed class ListCheckInsByFilterUseCase(
    ICheckInRepository checkInRepository,
    IPlanRepository planRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository,
    IPlanMemberRepository planMemberRepository)
    : IUseCase<ListCheckInsByFilterCommand>
{
    public async Task<IResult> HandleAsync(ListCheckInsByFilterCommand command)
    {
        try
        {
            command.Validate();

            var checkIns = new List<CheckIn>();

            if (command.PlanId is null)
            {
                checkIns = await checkInRepository.ListAllAsync();

                return Result.Success(
                    await Task.WhenAll(checkIns.Select(async checkIn => 
                        await checkIn.ToModelAsync(
                            planRepository,
                            habitRepository,
                            userRepository
                        )
                    ))
                );
            }

            var plan = await planRepository.GetByIdAsync(command.PlanId.Value);

            if (plan is null)
                return Result.Rejected(Messages.PlanNotFound);

            checkIns = await checkInRepository.ListByPlanIdAsync(command.PlanId.Value);

            if (command.UserId is not null)
            {
                var user = await userRepository.GetByIdAsync(command.UserId.Value);

                if (user is null)
                    return Result.Rejected(Messages.UserNotFound);

                var planMemberId = PlanMember.BuildId(
                    command.PlanId.Value,
                    command.UserId.Value
                );
                var planMember = await planMemberRepository.GetByIdAsync(planMemberId);

                if (planMember is null)
                    return Result.Rejected(Messages.PlanMemberNotFound);

                checkIns = checkIns.Where(checkIn => checkIn.UserId == command.UserId.Value).ToList();
            }

            if (command.Date is not null)
                checkIns = checkIns.Where(checkIn => checkIn.Date.ToDateTime(TimeOnly.MinValue) == command.Date.Value).ToList();

            if (command.Status is not null)
            {
                var status = CheckInStatus.Get(command.Status);
                checkIns = checkIns.Where(checkIn => checkIn.Status == status).ToList();
            }

            return Result.Success(
                await Task.WhenAll(checkIns.Select(async checkIn => 
                    await checkIn.ToModelAsync(
                        planRepository,
                        habitRepository,
                        userRepository
                    )
                ))
            );
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}