using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Challenges.ListChallengesByHabitId;

public class ListChallengesByHabitIdCommand : ICommand
{
	public ListChallengesByHabitIdCommand(Guid habitId) => HabitId = habitId;

	public Guid HabitId { get; init; }

	public void Validate()
	{
		if (HabitId == Guid.Empty)
			throw new ArgumentNullException(nameof(HabitId), Messages.EnterHabitId);
	}
}
