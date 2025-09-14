using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.Habits.Entities
{
	public class Habit : Entity
	{
		public string Name { get; set; }
		public DateOnly CreatedAt { get; set; }

		private Habit(string name)
		{
			Id = Guid.NewGuid().ToString();
			Name = name;
			CreatedAt = DateOnly.FromDateTime(DateTime.Today);
		}

		public static Habit CreateNew(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name), "Informe o nome do hábito");
			
			return new Habit(name);
		}
	}
}
