using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Penalties;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.AdminSettings.MoveInTime;

public sealed class MoveInTimeUseCase(
	IChallengeRepository challengeRepository,
	IChallengeMemberRepository challengeMemberRepository,
	ICheckInRepository checkInRepository,
	IPenaltyRepository penaltyRepository)
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

			var challenges = await challengeRepository.ListAllAsync();

			foreach (var challenge in challenges)
			{
				challenge.StartsAt = challenge.StartsAt.AddDays(daysToAdd);
				challenge.EndsAt = challenge.EndsAt.AddDays(daysToAdd);
				challenge.CreatedAt = challenge.CreatedAt.AddDays(daysToAdd);

				await challengeRepository.UpdateAsync(challenge);
			}

			var challengeMembers = await challengeMemberRepository.ListAllAsync();

			foreach (var challengeMember in challengeMembers)
			{
				challengeMember.JoinedAt = challengeMember.JoinedAt.AddDays(daysToAdd);
				challengeMember.CreatedAt = challengeMember.CreatedAt.AddDays(daysToAdd);

				await challengeMemberRepository.UpdateAsync(challengeMember);
			}

			var checkIns = await checkInRepository.ListAllAsync();

			foreach (var checkIn in checkIns)
			{
				checkIn.Date = checkIn.Date.AddDays(daysToAdd);
				checkIn.CreatedAt = checkIn.CreatedAt.AddDays(daysToAdd);
				checkIn.ReviewClosesAt = checkIn.ReviewClosesAt.AddDays(daysToAdd);
				checkIn.Reviews = checkIn.Reviews.ConvertAll(review =>
				{
					var shifted = Review.Create(review.ReviewerId, review.Status.Name, review.Date.AddDays(daysToAdd));
					return review.Enable ? shifted : shifted.Disable();
				});
                await checkInRepository.UpdateAsync(checkIn);
			}

			var penalties = await penaltyRepository.ListAllAsync();

			foreach (var penalty in penalties)
			{
				penalty.Date = penalty.Date.AddDays(daysToAdd);
				penalty.CreatedAt = penalty.CreatedAt.AddDays(daysToAdd);
				
				if (penalty.Date >= DateOnly.FromDateTime(DateTime.Today)){
					await penaltyRepository.DeleteAsync(penalty);
					continue;
				}
                await penaltyRepository.UpdateAsync(penalty);
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
