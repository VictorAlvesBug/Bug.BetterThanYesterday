using Bug.BetterThanYesterday.Application.DayOffs;
using Bug.BetterThanYesterday.Domain.DayOffs.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Plans.ValueObjects;
using Xunit;

namespace Bug.BetterThanYesterday.Application.Tests.DayOffs;

public class DayOffMapperTests
{
	[Fact]
	public void GetDaysOffAvailable_SubtractsUsedInRollingWindow()
	{
		var plan = Plan.Restore(
			Guid.NewGuid(),
			Guid.NewGuid(),
			Guid.NewGuid(),
			"Plan",
			new DateTime(2025, 1, 1),
			new DateTime(2025, 12, 31),
			PlanType.Public.Name,
			daysOffPerWeek: 2,
			penaltyValue: 10,
			isCancelled: false,
			createdAt: DateTime.Now);

		var reference = new DateOnly(2025, 6, 28);
		var dayOffs = new[]
		{
			DayOff.CreateNew(plan.Id, Guid.NewGuid(), reference.ToDateTime(TimeOnly.MinValue))
		};

		var available = DayOffMapper.GetDaysOffAvailable(plan, dayOffs, reference);

		Assert.Equal(1, available);
	}
}
