using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Moq;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public static class HabitRepositoryMockFactory
{
	public static (Mock<IHabitRepository> repo, List<Habit> data) Create()
	{
		List<Habit> habits = MockData.MockHabits;

		var habitRepository = new Mock<IHabitRepository>();

		habitRepository.Setup(repo => repo.AddAsync(It.IsAny<Habit>()));

		habitRepository
			.Setup(repo => repo.ListAllAsync())
			.ReturnsAsync(habits);

		habitRepository
			.Setup(repo => repo.UpdateAsync(It.IsAny<Habit>()));

		habitRepository
			.Setup(repo => repo.DeleteAsync(It.IsAny<Habit>()));

		habitRepository
			.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync((Guid habitId) => habits.Find(habit => habit.Id == habitId));

		habitRepository
			.Setup(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()))
			.ReturnsAsync((List<Guid> habitIds) => habits.Where(habit => habitIds.Contains(habit.Id)).ToList());

		habitRepository
			.Setup(repo => repo.GetByNameAsync(It.IsAny<string>()))
			.ReturnsAsync((string name) => habits.Find(habit => habit.Name == name));

		return (habitRepository, habits);
	}
}
