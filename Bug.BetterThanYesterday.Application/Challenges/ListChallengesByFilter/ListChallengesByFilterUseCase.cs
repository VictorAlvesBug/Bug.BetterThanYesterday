using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.ValueObjects;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

using Bug.BetterThanYesterday.Domain.ChallengeMembers;

namespace Bug.BetterThanYesterday.Application.Challenges.ListChallengesByFilter;

public class ListChallengesByFilterUseCase(
	IUserRepository userRepository,
	IChallengeRepository challengeRepository,
	IHabitRepository habitRepository,
	IChallengeMemberRepository challengeMemberRepository)
	: IUseCase<ListChallengesByFilterCommand>
{
	public async Task<IResult> HandleAsync(ListChallengesByFilterCommand command)
	{
		try
		{
			command.Validate();

			List<Challenge> challenges = [];

			if (command.OwnerId is not null && command.OwnerId.HasValue)
			{
				var owner = await userRepository.GetByIdAsync(command.OwnerId.Value);

				if (owner is null)
					return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

				challenges = await challengeRepository.ListByOwnerIdAsync(command.OwnerId.Value);
			}
			else
			{
				challenges = await challengeRepository.ListAllAsync();
			}
			
			if (command.HabitId is not null && command.HabitId.HasValue)
			{
				var habit = await habitRepository.GetByIdAsync(command.HabitId.Value);

				if (habit is null)
					return Result.Rejected(Messages.HabitNotFound, RejectionType.NotFound);

				challenges = challenges.Where(challenge => challenge.HabitId == command.HabitId.Value).ToList();
				//challenges = await challengeRepository.ListByHabitIdAsync(command.HabitId.Value);
			}

			var members = await challengeMemberRepository.ListAllAsync();
			var memberCountByChallengeId = members
				.GroupBy(member => member.ChallengeId)
				.ToDictionary(group => group.Key, group => group.Count());

			int MemberCount(Guid challengeId) =>
				memberCountByChallengeId.TryGetValue(challengeId, out var count) ? count : 0;

			foreach (var challenge in challenges)
				await ChallengeQuorumPostponer.PostponeWhileUnderQuorumAsync(challenge, MemberCount(challenge.Id), challengeRepository);

			if (!string.IsNullOrEmpty(command.Status))
			{
				if (!ChallengeStatus.TryGet(command.Status, out var status, out var errorMessage))
					return Result.Rejected(errorMessage ?? Messages.GenericError);

				challenges = challenges
					.Where(challenge => challenge.GetStatus(MemberCount(challenge.Id)) == status)
					.ToList();
			}

			if (!string.IsNullOrEmpty(command.Type))
			{
				if (!ChallengeType.TryGet(command.Type, out var type, out var errorMessage))
					return Result.Rejected(errorMessage ?? Messages.GenericError);

				challenges = challenges
					.Where(challenge => challenge.Type == type)
					.ToList();
			}

			var ownerIds = challenges.Select(challenge => challenge.OwnerId).Distinct().ToList();
			var owners = ownerIds.Count == 0
				? []
				: await userRepository.BatchGetByIdAsync(ownerIds);

			if (ownerIds.Count > owners.Count)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

			var ownersById = owners.ToDictionary(owner => owner.Id);

			var tasks = challenges.Select(async challenge =>
			{
				var habit = await habitRepository.GetByIdAsync(challenge.HabitId) ?? throw new Exception(Messages.HabitNotFound);
				return challenge.ToModel(habit, ownersById[challenge.OwnerId], MemberCount(challenge.Id));
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
