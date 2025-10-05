using Bug.BetterThanYesterday.Domain.Habits;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Moq;

namespace Bug.BetterThanYesterday.Application.Tests.Commons
{
	public static class HabitRepositoryMockFactory
	{
		public static (Mock<IHabitRepository> repo, List<Habit> data) Create()
		{
			var habitRepository = new Mock<IHabitRepository>();

			List<Habit> habits =
			[
				Habit.Restore(
					Guid.Parse("0160269d-1e78-4ca2-b100-ee42805b5c1e"),
					"Workout",
					new DateTime(1999, 01, 10)
				),
				Habit.Restore(
					Guid.Parse("f523e101-d4b9-453e-8669-c9e8a6918544"),
					"Reading",
					new DateTime(1967, 06, 20)
				),
				Habit.Restore(
					Guid.Parse("f8cfc6a0-7304-41bb-985e-a3ce9c955bde"),
					"Studing",
					new DateTime(2005, 04, 02)
				),
				Habit.Restore(
					Guid.Parse("809e7984-9eba-460e-be7d-955e229f7dce"),
					"Cooking",
					new DateTime(1991, 01, 16)
				)
			];

			habitRepository = new Mock<IHabitRepository>();

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
				.Setup(repo => repo.GetByNameAsync(It.IsAny<string>()))
				.ReturnsAsync((string name) => habits.Find(habit => habit.Name == name));

			return (habitRepository, habits);
		}
	}
}
