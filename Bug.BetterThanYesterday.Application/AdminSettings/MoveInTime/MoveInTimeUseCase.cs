using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.AdminSettings.MoveInTime;

public sealed class MoveInTimeUseCase(
	IPlanRepository planRepository,
	IPlanMemberRepository planMemberRepository,
	ICheckInRepository checkInRepository)
	: IUseCase<MoveInTimeCommand>
{
	public async Task<IResult> HandleAsync(MoveInTimeCommand command)
	{
		try
		{
			command.Validate();

			// Para simular um avanço no tempo, na verdade retrocedo as datas 
			// para simular que o dia atual foi movido para o futuro.
			// O mesmo se aplica ao retrocesso no tempo.
			var daysToAdd = -command.DaysAmount;

			var plans = await planRepository.ListAllAsync();

			foreach (var plan in plans)
			{
				plan.StartsAt = plan.StartsAt.AddDays(daysToAdd);
				plan.EndsAt = plan.EndsAt.AddDays(daysToAdd);
				plan.CreatedAt = plan.CreatedAt.AddDays(daysToAdd);

				await planRepository.UpdateAsync(plan);
			}

			var planMembers = await planMemberRepository.ListAllAsync();

			foreach (var planMember in planMembers)
			{
				planMember.JoinedAt = planMember.JoinedAt.AddDays(daysToAdd);
				planMember.CreatedAt = planMember.CreatedAt.AddDays(daysToAdd);

				await planMemberRepository.UpdateAsync(planMember);
			}

			var checkIns = await checkInRepository.ListAllAsync();

			foreach (var checkIn in checkIns)
			{
				checkIn.Date = checkIn.Date.AddDays(daysToAdd);
				checkIn.Reviews = checkIn.Reviews.ConvertAll(review =>
                    Review.Create(review.ReviewerId, review.Status.Name, review.Date.AddDays(daysToAdd)))
                    .ToList();
                await checkInRepository.UpdateAsync(checkIn);
			}

            var isDirectionForward = command.DaysAmount > 0;
			var absoluteDaysAmount = Math.Abs(command.DaysAmount);
			var dayOrDays = absoluteDaysAmount == 1 ? "dia" : "dias";

			if (isDirectionForward)
				return Result.Success($"Avançou {absoluteDaysAmount} {dayOrDays} no tempo");

			return Result.Success($"Retrocedeu {absoluteDaysAmount} {dayOrDays} no tempo");
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
