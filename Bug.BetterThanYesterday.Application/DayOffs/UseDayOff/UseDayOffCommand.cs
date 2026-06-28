using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.DayOffs.UseDayOff;

public class UseDayOffCommand : ICommand
{
	public required Guid PlanId { get; init; }
	public required Guid UserId { get; init; }
	public required DateTime Date { get; init; }

	public void Validate()
	{
		if (PlanId == Guid.Empty)
			throw new ArgumentNullException(nameof(PlanId), Messages.EnterPlanId);

		if (UserId == Guid.Empty)
			throw new ArgumentNullException(nameof(UserId), Messages.EnterUserId);

		if (Date == DateTime.MinValue)
			throw new ArgumentException(Messages.EnterDayOffDate, nameof(Date));
	}
}
