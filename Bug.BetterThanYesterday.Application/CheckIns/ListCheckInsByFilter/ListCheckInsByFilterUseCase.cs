using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.Extensions;
using Bug.BetterThanYesterday.Application.Ledger;
using Bug.BetterThanYesterday.Application.Penalties;
using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.ValueObjects;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.CheckIns.ListCheckInsByFilter;

public sealed class ListCheckInsByFilterUseCase(
    ICheckInRepository checkInRepository,
    IChallengeRepository challengeRepository,
    IHabitRepository habitRepository,
    IUserRepository userRepository,
    IChallengeMemberRepository challengeMemberRepository,
    IChallengeDayClosureProcessor challengeDayClosureProcessor,
    IChallengeFinancesProcessor challengeFinancesProcessor)
    : IUseCase<ListCheckInsByFilterCommand>
{
    public async Task<IResult> HandleAsync(ListCheckInsByFilterCommand command)
    {
        try
        {
            command.Validate();

            var checkIns = new List<CheckIn>();

            if (command.ChallengeId is null)
            {
                checkIns = await checkInRepository.ListAllAsync();

                return Result.Success(
                    await Task.WhenAll(checkIns.Select(async checkIn => 
                        await checkIn.ToModelAsync(
                            challengeRepository,
                            habitRepository,
                            userRepository
                        )
                    ))
                );
            }

            var challenge = await challengeRepository.GetByIdAsync(command.ChallengeId.Value);

            if (challenge is null)
                return Result.Rejected(Messages.ChallengeNotFound, RejectionType.NotFound);

            await challengeDayClosureProcessor.ProcessChallengeAsync(command.ChallengeId.Value);
            await challengeFinancesProcessor.ProcessAsync(command.ChallengeId.Value);

            checkIns = await checkInRepository.ListByChallengeIdAsync(command.ChallengeId.Value);

            await CheckInStatusResolver.ConsolidateClosedWindowCheckInsAsync(checkIns, checkInRepository);

            if (command.UserId is not null)
            {
                var user = await userRepository.GetByIdAsync(command.UserId.Value);

                if (user is null)
                    return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

                var challengeMemberId = ChallengeMember.BuildId(
                    command.ChallengeId.Value,
                    command.UserId.Value
                );
                var challengeMember = await challengeMemberRepository.GetByIdAsync(challengeMemberId);

                if (challengeMember is null)
                    return Result.Rejected(Messages.ChallengeMemberNotFound, RejectionType.NotFound);

                checkIns = checkIns.Where(checkIn => checkIn.UserId == command.UserId.Value).ToList();
            }

            if (command.Date is not null)
            {
                var filterDate = DateOnly.FromDateTime(command.Date.Value);
                checkIns = checkIns.Where(checkIn => checkIn.CalendarDate == filterDate).ToList();
            }

            if (command.Status is not null)
            {
                var status = CheckInStatus.Get(command.Status);
                checkIns = checkIns.Where(checkIn => checkIn.ResolveStatus() == status).ToList();
            }

            return Result.Success(
                await Task.WhenAll(checkIns.Select(async checkIn => 
                    await checkIn.ToModelAsync(
                        challengeRepository,
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