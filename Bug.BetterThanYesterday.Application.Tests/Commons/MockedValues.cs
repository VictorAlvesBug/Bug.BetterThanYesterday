using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users;
using Moq;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public class MockedValues
{
	public Mock<IUserRepository> UserRepository { get; set; }
	public Mock<IHabitRepository> HabitRepository { get; set; }
	public Mock<IPlanRepository> PlanRepository { get; set; }
	public List<User> Users { get; set; }
	public List<Habit> Habits { get; set; }
	public List<Plan> Plans { get; set; }
}
