using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users;
using Moq;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public class MockedValues
{
	public Mock<IUserRepository> UserRepository { get; set; }
	public Mock<IHabitRepository> HabitRepository { get; set; }
	public Mock<IChallengeRepository> ChallengeRepository { get; set; }
	public Mock<IChallengeMemberRepository> ChallengeMemberRepository { get; set; }
	public Mock<ICheckInRepository> CheckInRepository { get; set; }
	public List<User> Users { get; set; }
	public List<Habit> Habits { get; set; }
	public List<Challenge> Challenges { get; set; }
	public List<ChallengeMember> ChallengeMembers { get; set; }
	public List<CheckIn> CheckIns { get; set; }
}
